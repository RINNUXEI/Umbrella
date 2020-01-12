// Constants shared by both Unity and this App Script.
var CONST = {
  Method : "method",
  SheetName: "sheet",
  Data: "data",
  OrderBy: "orderBy",
  PlayerId: "id",
  PlayerName: "name",
  Score: "score",
  RequestRankingNumber: "rnum"
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
  
  if(method == "saveScore"){
    return saveScore(request);
  }else if(method == "getRanking") {
    return getRanking(request);
  }
  
  return ContentService.createTextOutput("Invalid method");
}

function saveScore(request) {
  var sheetName = request[CONST.SheetName];
  var data = request[CONST.Data];
  var jsonData = JSON.parse(data);

  var playerId = jsonData[CONST.PlayerId];
  var playerName = jsonData[CONST.PlayerName];
  var score = jsonData[CONST.Score];
  var requestRankingNum = jsonData[CONST.RequestRankingNumber];
  
  // Change score stirng to number type.
  score = +score;
  
  // Created time. Change the time zone of your region.
  var time = Utilities.formatDate(new Date(), TIME_ZONE, TIME_FORMAT);
  
  // Get the sheet.
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = spreadSheet.getSheetByName(sheetName);
  if(sheet == null) {
    // If sheet does not exist, create one.
    sheet = spreadSheet.insertSheet(sheetName);
    // Add the header.
    sheet.appendRow([ID_HEADER, NAME_HEADER, SCORE_HEADER, CREATED_TIME_HEADER]);
  }
  
  var sortData = [];
  
  var range = sheet.getDataRange();
  if(range.isBlank()){
    // If it's a blank sheet, create the header.
    sheet.appendRow([ID_HEADER, NAME_HEADER, SCORE_HEADER, CREATED_TIME_HEADER]);
  }
  
  // Get all sheet data.
  var sheetData = sheet.getDataRange().getValues();
  
  // Shift one line to exclude the header. 
  sheetData.shift();
  
  // Find the player with the provided ID.
  var findPlayer = false;
  for (var i = 0; i < sheetData.length; i++) {
    if (sheetData[i][ID_COLUMN] == playerId) {
      // If found, we over write the data.
      findPlayer = true;
      
      // +1 becase Google sheets index starts with 1.
      // +1 becase we excluded the header before, so totally +2 here.
      var row = i + 2;
      sheet.getRange(row, NAME_COLUMN + 1).setValue(playerName);
      sheet.getRange(row, SCORE_COLUMN + 1).setValue(score);
      sheet.getRange(row, CREATED_TIME_COLUMN + 1).setValue(time);
      
      // Update sort data.
      sheetData[i][NAME_COLUMN] = playerName;
      sheetData[i][SCORE_COLUMN] = score;
    }
    
    var sortObj = {id: sheetData[i][ID_COLUMN], name: sheetData[i][NAME_COLUMN], score: sheetData[i][SCORE_COLUMN], position: i};
    sortData.push(sortObj);
  }
  
  if(!findPlayer) {
    // If not found, append a new row.
    // Use appendRow to avoid lock cost cos it's an atom operation.
    sheet.appendRow([playerId, playerName, score, time]);
    sortData.push({id: playerId, name: playerName, score: score, position: sheetData.length});
  }
  
   if(jsonData[CONST.OrderBy] === "ASC") {
    sortData.sort(scoreCompareASC);
  }else{
    sortData.sort(scoreCompareDESC);
  }
  
  var sendData = createSendData(sortData, requestRankingNum);
  return ContentService.createTextOutput(JSON.stringify(sendData));
}

function getRanking(request) {
  var sheetName = request[CONST.SheetName];
  var data = request[CONST.Data];
  var jsonData = JSON.parse(data);
  
  var requestRankingNum = jsonData[CONST.RequestRankingNumber];
  
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = spreadSheet.getSheetByName(sheetName);
  if(sheet == null) {
    return ContentService.createTextOutput("Invalid sheet name");
  }
  
  var sortData = [];
  
  var sheetData = sheet.getDataRange().getValues();
  sheetData.shift();
  
  for (var i = 0; i < sheetData.length; i++) {
    var sortObj = {id: sheetData[i][ID_COLUMN], name: sheetData[i][NAME_COLUMN], score: sheetData[i][SCORE_COLUMN], position: i};
    sortData.push(sortObj);
  }
  
  if(jsonData[CONST.OrderBy] === "ASC") {
    sortData.sort(scoreCompareASC);
  }else{
    sortData.sort(scoreCompareDESC);
  }
  
  var sendData = createSendData(sortData, requestRankingNum);
  return ContentService.createTextOutput(JSON.stringify(sendData));
}

// Sort the scores in ascending order, if two scores are equal, the early created one will come first.
function scoreCompareASC(a, b) {
  if(a.score == b.score)
    return a.position - b.position;
  if(a.score > b.score)
    return 1;
  return -1;
}

// Sort the scores in descending order, if two scores are equal, the early created one will come first.
function scoreCompareDESC(a, b) {
  if(a.score == b.score)
    return a.position - b.position;
  if(a.score < b.score)
    return 1;
  return -1;
}

// Format the sending data.
function createSendData(data, num) {
  var sendData = [];
  for(var i = 0; i < data.length; i++)
  {
    if(i >= num) break;
    var sendObj = {id: data[i].id, name: data[i].name, score: data[i].score};
    sendData.push(sendObj);
  }
  
  return sendData;
}