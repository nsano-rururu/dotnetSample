@ECHO OFF

REM 引数の値チェック
IF "%~1"=="" (
	GOTO NOT_ARGS_END
) ELSE (
	SET CHECKFILE=%1
)

REM ファイルの存在チェック
IF NOT EXIST %CHECKFILE% GOTO FILE_NOT_FOUND_END

REM ファイルが存在した場合の処理
EXIT /b 0

REM 引数無し場合の処理
:NOT_ARGS_END
EXIT /b 1

REM ファイルが存在しない場合の処理
:FILE_NOT_FOUND_END
EXIT /b 2
