// Constants shared by Unity and this App Script.
var CONST = {
  Method : "method",
  SheetName: "sheet",
  Data: "data",
  UserId: "userId",
  KeyValuePairs: "kvps",
  Keys : "keys",
  SaveData: "saveData",
  GetData: "getData",
  CellReference: "cellRef"
};

function doPost(e) {
  var request = e.parameter;
  var method = request[CONST.Method];
  
  if(method == CONST.SaveData){
    return saveData(request);
  }else if(method == CONST.GetData) {
    return getData(request);
  }
  
  return ContentService.createTextOutput("Error: Invalid method");
}

function saveData(request) {
  var sheetName = request[CONST.SheetName];
  var data = request[CONST.Data];
  var jsonData = JSON.parse(data);

  var userId = jsonData[CONST.UserId];
  var kvps = jsonData[CONST.KeyValuePairs];
  
  // Created time
  var time = Utilities.formatDate(new Date(), "GMT+9", "yyyy/MM/dd HH:mm:ss");
  
  // Get the sheet
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = spreadSheet.getSheetByName(sheetName);
  if(sheet == null) {
    // Insert sheet if not exists
    sheet = spreadSheet.insertSheet(sheetName);
  }
  
  // Retrieve all data
  var sheetData = sheet.getDataRange().getValues();
  // Header data
  var header = sheetData[0];
  
  // Find whether user id already exists
  var row = findRowByUserId(sheetData, userId);
  if(row != 0){
    // User id already exists
    for(var key in kvps){
      var value = kvps[key];
      // Find whether the key of the data to be saved aleardy exists
      var column = findColumnByKey(header, key);
      if(column != 0){
        // The key already exists, overwrite the value
        sheet.getRange(row, column).setValue(value);
      }else{
        // Adds a new key column
        header.push(key);
        sheet.getRange(1, header.length).setValue(key);
        // Appends the value of the key
        sheet.getRange(row, header.length).setValue(value);
      }
    }
  }else{
     // Insert header title if it has not been set
    if(header == null || header.length < 2){
      var values = [["userId", "updateTime"]];
      sheet.getRange(1, 1, 1, 2).setValues(values);
      header = values[0];
    }
                                    
    // Appends a new row
    var content = [];
    for(var i = 0; i < header.length; i++) content.push("");
    content[0] = userId;
    content[1] = time;
    for(var key in kvps){
      var value = kvps[key];
      var column = findColumnByKey(header, key);
      if(column != 0){
        content[column - 1] = value;
      }else{
        // Adds a new key column
        header.push(key);
        sheet.getRange(1, header.length).setValue(key);
        // Appends the value
        content.push(value);
      }
    }
    sheet.appendRow(content);
  }
  
  return ContentService.createTextOutput("Save data succeeded");
}

function getData(request) {
  var sheetName = request[CONST.SheetName];
  var data = request[CONST.Data];
  var jsonData = JSON.parse(data);
  
  var userId = jsonData[CONST.UserId];
  var keys = jsonData[CONST.Keys];
  
  var cellRef = jsonData[CONST.CellReference];
  
  var spreadSheet = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = spreadSheet.getSheetByName(sheetName);
  if(sheet == null) {
    return ContentService.createTextOutput("Error: Invalid sheet name");
  }
  
  if(cellRef){
    var aa = sheet.getRange(cellRef).getValues();
    var a = []
    for(var i = 0; i < aa.length; i++){
      a = a.concat(aa[i]);
    }
    return ContentService.createTextOutput(JSON.stringify(a));
  }
  
  var sheetData = sheet.getDataRange().getValues();
  var header = sheetData[0]
  
  // Find whether user id already exists
  var row = findRowByUserId(sheetData, userId);
  if(row == 0){
    return ContentService.createTextOutput("Error: Invalid user id");
  }
  
  var ret = [];
  for(var i = 0; i < keys.length; i++){
    var column = findColumnByKey(header, keys[i]);
    ret.push(column == 0 ? "#NULL#" : sheetData[row - 1][column - 1]);
  }
  
  return ContentService.createTextOutput(JSON.stringify(ret));
}

function findRowByUserId(sheetData, userId) {
  var row = 0;
  for(var i = 1; i < sheetData.length; i++){
    if(sheetData[i][0] == userId){
      row = i + 1;
      break;
    }
  }
  return row;
}

function findColumnByKey(header, key) {
  var column = 0;
  for(var i = 2; i < header.length; i++){
    if(header[i] == key){
      column  = i + 1;
      break;
    }
  }
  return column;
}
