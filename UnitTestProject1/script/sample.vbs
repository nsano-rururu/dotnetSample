Dim oParam  
Set oParam = WScript.Arguments  

'�����̐���0�̏ꍇ�A�߂�l�u1�v�ŏ������I��
If oParam.Count = 0 Then
	Set oParam = Nothing	'�I�u�W�F�N�g�̔j��
	Set oParam = Nothing	'�I�u�W�F�N�g�̔j��
	WScript.Quit(1)
End If

'�����̐���1�ł͂Ȃ��ꍇ�A�߂�l�u2�v�ŏ������I��
If oParam.Count <> 1 Then
	Set oParam = Nothing	'�I�u�W�F�N�g�̔j��
	WScript.Quit(2)
End If

'�����Ɏw�肳�ꂽ�l(�t�@�C��)�����݂��Ȃ��ꍇ�A�߂�l�u3�v�ŏ������I��
Set objFileSys = CreateObject("Scripting.FileSystemObject")
If objFileSys.FileExists(oParam(0)) = False Then
	Set oParam = Nothing		'�I�u�W�F�N�g�̔j��
	Set objFileSys = Nothing	'�I�u�W�F�N�g�̔j��
	WScript.Quit(3)
End If
Set objFileSys = Nothing		'�I�u�W�F�N�g�̔j��

'����I��
Set oParam = Nothing			'�I�u�W�F�N�g�̔j��
WScript.Quit(0)
