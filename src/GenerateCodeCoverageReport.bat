@echo off
SETLOCAL

SET exclude=\"[*]*.Exceptions.*,[*]*HE.CRM.*,[*]HE.InvestmentLoans.CRM.*,[*]*.Contract.*\"
 
dotnet test /p:CollectCoverage=true  /p:CoverletOutputFormat=cobertura /p:Exclude=%exclude% /p:MergeWith='/path/to/result.json'
reportgenerator -reports:*.Tests/coverage.cobertura.xml* -reporttypes:"HTML;HTMLSummary;Cobertura;" -targetdir:%1tests/CodeCoverageReport -historydir:%1tests/CodeCoverageReportHistory -assemblyfilters:"-*.TestUtils;-*.Tests" 

pause

ENDLOCAL