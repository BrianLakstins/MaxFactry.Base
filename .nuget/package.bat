rem Package the library for Nuget
copy ..\MaxFactry.Base-NF-2.0\bin\Release\MaxFactry.Base*.dll lib\net20\
copy ..\MaxFactry.Base-NF-4.5.2\bin\Release\MaxFactry.Base*.dll lib\net452\
copy ..\MaxFactry.Base-NC-2.1\bin\Release\netcoreapp2.1\MaxFactry.Base*.dll lib\netcoreapp2.1\

c:\install\nuget\nuget.exe pack MaxFactry.Base.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 