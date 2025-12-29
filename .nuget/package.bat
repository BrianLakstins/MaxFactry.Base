rem Package the library for Nuget
del lib\*.dll /s /q
copy ..\MaxFactry.Base-NF-2.0\bin\Release\MaxFactry.Base*.dll lib\net20\
copy ..\MaxFactry.Base-NF-4.5.2\bin\Release\MaxFactry.Base*.dll lib\net452\
copy ..\MaxFactry.Base-NF-4.7.2\bin\Release\MaxFactry.Base*.dll lib\net472\
copy ..\MaxFactry.Base-NF-4.8\bin\Release\MaxFactry.Base*.dll lib\net48\
copy ..\MaxFactry.Base-NC-2.1\bin\Release\netcoreapp2.1\MaxFactry.Base*.dll lib\netcoreapp2.1\
copy ..\MaxFactry.Base-NC-3.1\bin\Release\netcoreapp3.1\MaxFactry.Base*.dll lib\netcoreapp3.1\
copy ..\MaxFactry.Base-NC-6.0\bin\Release\net6.0\MaxFactry.Base*.dll lib\net6.0\
copy ..\MaxFactry.Base-NC-8.0\bin\Release\net8.0\MaxFactry.Base*.dll lib\net8.0\

c:\install\nuget\nuget.exe pack MaxFactry.Base.nuspec -OutputDirectory "packages" -IncludeReferencedProjects -properties Configuration=Release 