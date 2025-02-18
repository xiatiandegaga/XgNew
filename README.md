# Xg
framework:
.net 8
module
efcore-codefirst
redis

1:backend start
  Preconditions
    postgresql
    redis

add them in the file of appsettings.json,then run the file of auto-migration.bat in the directory \backend\src\Services\IdentityService\IdentityApi\
it will fill the seeddata to the db


2:front start 
  npm install 
  npm run dev

doc url:https://tdesign.tencent.com/starter/docs/vue-next/get-started







