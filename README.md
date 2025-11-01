<h1>Flight Sim Ventures</h1>
This is an proof of concept I created few years ago that essentially is a more modern version of a flight sim online service called FSEconomy (https://www.fseconomy.net).
There are 3 components to the system which I had in seperate repos, but combined into a mono.  If someone is looking for a starting point for how to make your own version, this is the only complete example you might find.

1. API - the database and api layer the other clients talk to.
2. Web - the primary app used for interacting with the economy.
3. Client - the app that runs on the user's host machine.  It uses microsoft sim connect library to gather telemetry and relay the player state back to the API.

Everything actually does work, but it's very bare bones.  Right now the client app only functions on Windows but I don't think it would be much effort to get a cross platform version working.

Some old screenshots of it working...

<img width="758" height="596" alt="2020-02-02 16_14_55-Flight Sim Economy" src="https://github.com/user-attachments/assets/4e48da4c-8958-44af-8723-319140b82e59" />
<img width="1154" height="622" alt="2020-02-22 00_22_15-EconomyWeb" src="https://github.com/user-attachments/assets/24f5fb6c-22d7-4b61-82df-fa0197a46858" />
<img width="2560" height="2560" alt="localhost_5000_swagger_index html (4)" src="https://github.com/user-attachments/assets/58891515-741c-46be-9a01-88eca489882d" />
