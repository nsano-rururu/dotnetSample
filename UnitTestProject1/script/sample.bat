@ECHO OFF

REM �����̒l�`�F�b�N
IF "%~1"=="" (
	GOTO NOT_ARGS_END
) ELSE (
	SET CHECKFILE=%1
)

REM �t�@�C���̑��݃`�F�b�N
IF NOT EXIST %CHECKFILE% GOTO FILE_NOT_FOUND_END

REM �t�@�C�������݂����ꍇ�̏���
EXIT /b 0

REM ���������ꍇ�̏���
:NOT_ARGS_END
EXIT /b 1

REM �t�@�C�������݂��Ȃ��ꍇ�̏���
:FILE_NOT_FOUND_END
EXIT /b 2
