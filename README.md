# Xg
framework:
.net8  efcore-codefirst  redis vue3 vite

1:backend start

  Preconditions
    postgresql redis

add them in the file of appsettings.json,then run the file of auto-migration.bat in the directory \backend\src\Services\IdentityService\IdentityApi\
it will fill the seeddata to the db

http://localhost:5001/swagger/

2:front start

  npm install   npm run dev
  
  http://localhost:8000/
  account: test
  password: admin2025
  
![image](https://github.com/user-attachments/assets/4556dd90-21d6-46fc-ab57-111be6e2897d)

doc url:https://tdesign.tencent.com/starter/docs/vue-next/get-started







