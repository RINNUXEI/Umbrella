# Umbrella
A Unity asset using Google Sheets for storing and retrieving data. This package also provides two applications, **Database** and **Ranking**, to demostrate the usage. 

Database can let you send any form of data to Google sheets and retrieve them later. You can use this system to store player information, such as id, name, equipments, and even master data of your game systems. Data are sent by key value paris and are restricted to some simiple types that [MiniJson](https://gist.github.com/darktable/1411710) can parse.

Ranking is another application that can send player scores to Google sheets and get the ranking list back.

Because this asset almost does nothing about security things (hope Google will handle it well though), as well as [Quotas for Google Services](https://developers.google.com/apps-script/guides/services/quotas#current_quotas), **it is not recommended for big scale or any commercial projects**. Indie developers who just want a simple way of saving and getting data online are welcome to try this :)

# How to use
## Google sheets side
1. Create a new Google sheet.
2. Open the Script Editor from Tools > Script Editor.
3. Copy the content of *Assets/Umbrella/Databse/Database.gs* (for Database) or *Assets/Umbrella/Ranking/Ranking.gs* (for Ranking) to Code.gs within the Script Editor.
4. Save the project and give it a name.
5. From the menu, Publish < Deploy as web app...
6. In the Deploy as web app popup, make sure you Execute the app as your own account and **Who has access to the app** is set to *Anyone, even anonymous*.
7. Click Deploy and copy the web app URL.
8. If *This app isn't verified* pops up, at the left bottom corner, click *Advanced* > *Go to [your_project_name] (unsafe)* > *Allow* to verify your app. 
## Unity side (Database)
1. Drag & Drop *Assets/Umbrella/Database/DatabaseManager* prefab into your scene hierarchy (the scene you want to do the data communication).
2. In the Inspector of *Assets/Umbrella/Database/DatabaseSettings* scriptableobject, paste the web app URL you copied previously to the *App URL* field.
3. Enter the sheet name of your default sheet into the *Default Sheet Name* field.
4. You can now send any data to Google sheets using `DatabaseManager.Instance.SendDataAsync(data, sheetName)` and get data from Google sheets using `DatabaseManager.Instance.GetDataAsync(keys, responseHandler, sheetName)`. If you omit the *sheetName* parameter, the *Default Sheet Name* will be used. 
5. *responseHandler* is a callback function which will be called when response returns. Its parameter is a list of generic objects which contains the values of requested keys. You can use this callback to cast the values and implment your own display logic.
6. If you want, add `yield return` before `DatabaseManager.Instance.SendDataAsync(data, sheetName)` or `DatabaseManager.Instance.GetDataAsync(keys, responseHandler, sheetName)` to hang on and wait until the *responseHandler* callback returns.
7. For specific usage, please refer to the sample scene and scripts.
## Unity side (Ranking)
1. Drag & Drop *Assets/Umbrella/Ranking/RankingManager* prefab into your scene hierarchy (the scene you want to do the data communication).
2. In the Inspector of *Assets/Umbrella/Ranking/RankingSettings* scriptableobject, paste the web app URL you copied previously to the *App URL* field.
3. Enter the default settings for your ranking requests. 
    - *Ranking Name* refers to the name of the ranking, which also determines the name of the corresponding Google Sheets sheet.
    - *Top Ranking List Settings* 
        - *Take Number* tells how many data you want to take from the top ranking list, if zero no data will be returned (the ranking list won't be sorted at all).
        - *Order By* tells whether sort the ranking list in ascending or descending order.
    - *Around Me Ranking List Settings* 
        - *Take Number* tells how many data you want to take from the around me ranking list, if zero no data will be returned (the ranking list won't be sorted at all).
        - *Order By* tells whether sort the ranking list in ascending or descending order.
4. Each time you want to send a score to Google Sheets, you need a `SendScoreRequestData` object. You can easily create a default one with the same settings as you set in the *RankingSettings* scriptableobject by calling `RankingManager.Instance.CreateDefaultSendScoreRequest(score)`. The same rule also applies to getting ranking lists, which requires a `RankingRequestData` object. You can now call `RankingManager.Instance.SendScoresAsync(requestDataList, responseHandler)` to send scores and call `RankingManager.Instance.GetRankingListsAsync(requestDataList, responseHandler)` to get ranking lists. 
5. *responseHandler* is a callback function which will be called when response returns. Its parameter is a list of `RankingResponseData` objects which contains the sorted ranking lists. You can use this callback to implment your own ranking result display logic.
6. If you want, add `yield return` before `RankingManager.Instance.SendScoresAsync(requestDataList, responseHandler)` or `RankingManager.Instance.GetRankingListsAsync(requestDataList, responseHandler)` to hang on and wait until the *responseHandler* callback returns.
7. For specific usage, please refer to the sample scene and scripts.

# Demo
## Database
* Send data to Google sheets. The gif shows sending data using key value pairs.
![Send data to Google sheets](Demos/send_data.gif)

* Update data in Google sheets. The gif below shows updating data by sending the key and a new value.
![Update data in Google sheets](Demos/update_data.gif)

* Send another data. Note: Because Umbrella generates unique ids in Unity and saves them in PlayerPrefs, you should clear PlayerPrefs otherwise duplicated sending will overwrite the data already stored in Google sheets.
![Send another data](Demos/send_another_data.gif)

* Get data from Google sheets by keys. The gif below shows getting data using a list of keys.
![Get data from Google sheets by keys](Demos/get_data.gif)

* You can also get data by cell references.
![Get data from Google sheets by cell references](Demos/get_data_by_cell.gif)

## Ranking
* Send scores to Google sheets.
![Send scores to Google sheets](Demos/send_score.gif)

* Update score in Google sheets.
![Update score in Google sheets](Demos/update_score.gif)

* Get ranking list from Google sheets.
![Get ranking list from Google sheets](Demos/get_ranking.gif)


