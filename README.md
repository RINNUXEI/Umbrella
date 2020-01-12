# Umbrella
A Unity asset using Google Sheets for storing and retrieving data.

# How to use
## Google sheets side
1. Create a new Google sheet.
2. Open the Script Editor from Tools > Script Editor.
3. Copy the content of the .gs file (within the .unitypackage) to the Script Editor.
4. Save the project and give it a name.
5. From the menu, Publish < Deploy as web app...
6. In the Deploy as web app popup, make sure you Execute the app as your own account and **Who has access to the app** is set to *Anyone, even anonymous*.
7. Click Deploy and copy the web app URL.
## Unity side (Database)
1. Drag & Drop the *DatabaseManager* prefab into your scene hierarchy (the scene you want to do the data communication).
2. From the prefab instance, paste the web app URL you copied previously to the *App Url* field.
3. Enter the sheet name of your default sheet into the *Default Sheet* field.
4. You can now send any data to Google sheets using `DatabaseManager.Instance.SendDataAsync(data, handleResponseCallback, sheetName)` and get data from Google sheets use `DatabaseManager.Instance.GetDataAsync(key, handleResponseCallback, sheetName)`.
5. For specific usage, please refer to the sample scene and scripts.
## Unity side (Ranking)
1. Drag & Drop the *RankingManager* prefab into your scene hierarchy (the scene you want to do the data communication).
2. In the *RankingSettings* scriptableobject asset, paste the web app URL you copied previously to the *App Url* field.
3. You can set different kinds of rankings by adding entries to the *Ranking Request Settings* array. *Ranking Name* refers to the name of the ranking, *Ranking Number* tells how many rows you want to get (e.g., ranking number of 3 will get you only the first three places of the ranking list), and *Order By* allows you to choose from ascending (ASC) or descending (DESC) order.
4. You can now send score to Google sheets using `RankingManager.Instance.SendScoreAsync(playerName, score, handleResponseCallback, rankingRequestIndex)` and get the ranking list from Google sheets using `RankingManager.Instance.GetRankingListAsync(handleResponseCallback, rankingRequestIndex)`.
5. For specific usage, please refer to the sample scene and scripts.

