Dim oParam  
Set oParam = WScript.Arguments  

'引数の数が0の場合、戻り値「1」で処理を終了
If oParam.Count = 0 Then
	Set oParam = Nothing	'オブジェクトの破棄
	Set oParam = Nothing	'オブジェクトの破棄
	WScript.Quit(1)
End If

'引数の数が1ではない場合、戻り値「2」で処理を終了
If oParam.Count <> 1 Then
	Set oParam = Nothing	'オブジェクトの破棄
	WScript.Quit(2)
End If

'引数に指定された値(ファイル)が存在しない場合、戻り値「3」で処理を終了
Set objFileSys = CreateObject("Scripting.FileSystemObject")
If objFileSys.FileExists(oParam(0)) = False Then
	Set oParam = Nothing		'オブジェクトの破棄
	Set objFileSys = Nothing	'オブジェクトの破棄
	WScript.Quit(3)
End If
Set objFileSys = Nothing		'オブジェクトの破棄

'正常終了
Set oParam = Nothing			'オブジェクトの破棄
WScript.Quit(0)
