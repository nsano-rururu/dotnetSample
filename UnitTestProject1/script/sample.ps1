Param([string] $name, [string] $filePath)

# 名前付き引数「name」の値の長さが0の場合、戻り値：1を指定して処理終了
if ($name.Trim().Length -eq 0)
{
    return 1
}
# 名前付き引数「filePath」の値の長さが0の場合、戻り値：2を指定して処理終了
if ($filePath.Trim().Length -eq 0)
{
    return 2
}
# 名前付き引数「filePath」(ファイル)が存在しない場合、戻り値：3を指定して処理終了
if (-not(Test-Path $filePath))
{
    return 3
}

# 戻り値：0を指定して処理終了
return 0
