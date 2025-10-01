Diskinfo is dotnet format of crystaldiskinfo 50%

added
1. Extract Device information from sm management , using json properties 
2. Anycpu 
3. vs studio 2022 dev powershell
4. digital signeture at [DiskInfoDotnet.Related]
5. code optimized
6. device information
7. using task
8. delay sign in +
9. has some more things

To run it on windows xp u have to compile the project with net40 , take a look at dotnetfour.prop , it should be successfull compile with dotnet 4 but will works or not idont know because didnt tested yet ,

select x86 or x64 bit , do not run at AnyCPU , it will be crash or u will get corrupted information , 

u can get information in two format cmd ; args 
```
exe -E ex {u will get all information which extracted }
exe -O op {u will get optimized information except bool 0 and null }
```

example 
<img width="1920" height="1030" alt="{3F5CD021-19C8-4837-99AD-B5A4740ACD54}" src="https://github.com/user-attachments/assets/214340bd-0b31-47db-93dc-b1bd61271f82" />


