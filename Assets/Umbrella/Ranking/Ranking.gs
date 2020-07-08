// Constants shared by both Unity and this App Script.
var CONST = {
  Method : "method",
  Payload: "payload",
  SaveScoreMethod : "saveScore",
  GetRankingMethod : "getRanking",
  PlayerId: "id",
  PlayerName : "playerName",
  PlayerScore : "score",
  RankingRequest : "request",
  RankingName : "rankingName",
  TopRankingSettings : "top",
  AroundMeRankingSettings : "aroundMe",
  RankingTakeNumber : "number",
  RankingOrderBy : "orderBy",
};

// Constants particularly used in this App Script.
var ID_COLUMN = 0;
var NAME_COLUMN = 1;
var SCORE_COLUMN = 2;
var CREATED_TIME_COLUMN = 3;
var ID_HEADER = "PlayerId";
var NAME_HEADER = "PlayerName";
var SCORE_HEADER = "Score";
var CREATED_TIME_HEADER = "CreatedTime";
var TIME_ZONE = "GMT+9";
var TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";

function doPost(e) {
  var request = e.parameter;
  var method = request[CONST.Method];
  
  if(method == CONST.SaveScoreMethod){
    return saveScores(request[CONST.Payload]);
  }else if(method == CONST.GetRankingMethod){
    return getRankings(request[CONST.Payload]);
  }
  
  return ContentService.createTextOutput("Invalid method");
}

function saveScores(payload){
  var jsonData = JSON.parse(payload);
  var id = jsonData[CONST.PlayerId];
  var playerName = jsonData[CONST.PlayerName];
  var rankingReq = jsonData[CONST.RankingRequest];
  
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  
  var results = [];
  for(var i in rankingReq){
    var req = rankingReq[i];
    var score = req[CONST.PlayerScore];
    var rankingName = req[CONST.RankingName];
    var topSettings = req[CONST.TopRankingSettings];
    var aroundMeSettings = req[CONST.AroundMeRankingSettings];
    var result = saveScore(spreadSheet, id, playerName, score, rankingName, topSettings, aroundMeSettings);
    results.push({rankingName: rankingName, top: result.top, aroundMe: result.aroundMe});
  }
  
  return ContentService.createTextOutput(JSON.stringify(results));
}

function saveScore(spreadSheet, id, playerName, score, rankingName, topSettings, aroundMeSettings){
  // Get the sheet.
  var sheet = spreadSheet.getSheetByName(rankingName);
  if(sheet == null) {
    // If sheet does not exist, create one.
    sheet = spreadSheet.insertSheet(rankingName);
    // Add the header.
    sheet.appendRow([ID_HEADER, NAME_HEADER, SCORE_HEADER, CREATED_TIME_HEADER]);
  }
  
  var range = sheet.getDataRange();
  if(range.isBlank()){
    // If it's a blank sheet, create the header.
    sheet.appendRow([ID_HEADER, NAME_HEADER, SCORE_HEADER, CREATED_TIME_HEADER]);
  }
  
  // Change score stirng to number type.
  score = +score;
  
  // Created time. Change the time zone of your region.
  var time = Utilities.formatDate(new Date(), TIME_ZONE, TIME_FORMAT);
  
  var sortData = [];
  // Get all sheet data.
  var sheetData = range.getValues();
  // Find the player with the provided ID.
  var findPlayer = false;
  for (var i = 1; i < sheetData.length; i++) {
    if (sheetData[i][ID_COLUMN] == id) {
      // If found, overwrite the data.
      findPlayer = true;
      
      // +1 because Google sheets index starts with 1.
      var row = i + 1;
      sheet.getRange(row, NAME_COLUMN + 1).setValue(playerName);
      sheet.getRange(row, SCORE_COLUMN + 1).setValue(score);
      sheet.getRange(row, CREATED_TIME_COLUMN + 1).setValue(time);
      
      // Update sort data.
      sheetData[i][NAME_COLUMN] = playerName;
      sheetData[i][SCORE_COLUMN] = score;
    }
    
    var sortObj = {id: sheetData[i][ID_COLUMN], playerName: sheetData[i][NAME_COLUMN], score: sheetData[i][SCORE_COLUMN], pos: i};
    sortData.push(sortObj);
  }
  
  if(!findPlayer) {
    // If not found, append a new row.
    // Use appendRow to avoid lock cost cos it's an atom operation.
    sheet.appendRow([id, playerName, score, time]);
    sortData.push({id: id, playerName: playerName, score: score, pos: sheetData.length});
  }
  
  return getRankingLists(sortData, id, topSettings, aroundMeSettings);
}

function getRankings(payload){
  var jsonData = JSON.parse(payload);
  var id = jsonData[CONST.PlayerId];
  var rankingReq = jsonData[CONST.RankingRequest];
  
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  
  var results = [];
  for(var i in rankingReq){
    var req = rankingReq[i];
    var rankingName = req[CONST.RankingName];
    var topSettings = req[CONST.TopRankingSettings];
    var aroundMeSettings = req[CONST.AroundMeRankingSettings];
    var result = getRanking(spreadSheet, id, rankingName, topSettings, aroundMeSettings);
    if(result != null){
      results.push({rankingName: rankingName, top: result.top, aroundMe: result.aroundMe});
    }
  }
  
  return ContentService.createTextOutput(JSON.stringify(results));
}

function getRanking(spreadSheet, id, rankingName, topSettings, aroundMeSettings){
  var sheet = spreadSheet.getSheetByName(rankingName);
  if(sheet == null){
    return null;
  }
  
  var sheetData = sheet.getDataRange().getValues();
  
  var sortData = [];
  // Ranking data begins from the second line.
  for (var i = 1; i < sheetData.length; i++) {
    var sortObj = {id: sheetData[i][ID_COLUMN], playerName: sheetData[i][NAME_COLUMN], score: sheetData[i][SCORE_COLUMN], pos: i};
    sortData.push(sortObj);
  }
  
  return getRankingLists(sortData, id, topSettings, aroundMeSettings);
}

function getRankingLists(data, id, topSettings, aroundMeSettings){
  var topRankingList = [];
  var aroundMeRankingList = [];
  
  var topTakeNumber = topSettings[CONST.RankingTakeNumber];
  var topOrderBy = topSettings[CONST.RankingOrderBy];
  if(topTakeNumber > 0){
    sortRankingList(data, topOrderBy);
    topRankingList = data.slice(0, topTakeNumber);
  }
  
  var aroundMeTakeNumber = aroundMeSettings[CONST.RankingTakeNumber];
  var aroundMeOrderBy = aroundMeSettings[CONST.RankingOrderBy];
  if(aroundMeTakeNumber > 0){
    if(topTakeNumber <= 0){
      // If data not sorted, sort it by around me ranking order-by.
      sortRankingList(data, aroundMeOrderBy);
    }
    else if(topOrderBy != aroundMeOrderBy){
      // If data already sorted, we need make a deep copy before sorting it again, otherwise the top ranking list position will be modified.
      data = deepCopyRankingList(data);
      sortRankingList(data, aroundMeOrderBy);
    }
    var range = findAroundMeRankingRange(data, id, aroundMeTakeNumber);
    if(range != null){
      aroundMeRankingList = data.slice(range.start, range.end + 1);
    }
  }
  
  return {top: topRankingList, aroundMe: aroundMeRankingList};
}

function sortRankingList(data, orderBy){
  if(orderBy == "ASC") {
    data.sort(scoreCompareASC);
  }else{
    data.sort(scoreCompareDESC);
  }
  
  // Set pos to corresponding ranking position.
  for(var i = 0; i < data.length; i++){
    data[i].pos = i + 1;
  }
}

// Sort the scores in ascending order, if two scores are equal, the early created one will come first.
function scoreCompareASC(a, b) {
  if(a.score == b.score)
    return a.pos - b.pos;
  return a.score - b.score;
}

// Sort the scores in descending order, if two scores are equal, the early created one will come first.
function scoreCompareDESC(a, b) {
  if(a.score == b.score)
    return a.pos - b.pos;
  return b.score - a.score;
}

function findAroundMeRankingRange(data, myId, num) {
  // Find my position.
  var myPos = data.findIndex(d => d.id == myId);
  if(myPos < 0) return null;
  
  var count = 0;
  var left = myPos;
  var right = myPos + 1;
  
  // Expand range from my position to each side until either side reaches its end.
  while(left >= 0 && right < data.length && count < num){
    count += 2;
    left--;
    right++;
  }
  
  // If still needs elements, expand to the left side until reaching the start.
  while(count < num && left >= 0){
    count++;
    left--;
  }
  
  // If still needs elements, expand to the right side until reaching the end.
  while(count < num && right < data.length){
    count++;
    right++;
  }
  
  // Move back each side to match indices.
  left++;
  right--;
  
  return {start: left, end: right};
}

function deepCopyRankingList(data){
  var list = [];
  for(var i in data){
    list.push({id: data[i].id, playerName: data[i].playerName, score: data[i].score, pos: data[i].pos});
  }
  return list;
}
