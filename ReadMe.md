# Build Instructions:
1. Build using the build.bat script or by loading the project and using the menu Build/WebGL
2. Copy the contents of Builds/WebGL to the folder where the static content should be served from.
Done!

# Seeding the Database:
1. Open your unity scene containing the SUGAR Unity Manager.
2. Make sure the Base Address is correct.
3. Make sure the Game Token and Game Id exist in the database. If not, add them.
4. In the Menus: Tools/SUGAR/Seed Game.
5. Enter the admin login details.
6. Click Seed Game.

Note: You will also need to have the users and groups defined in the Canvas/Controller component created with matching Ids as well as a small amount of "chocolate" allocated to each.

You can find a dump of the required tables in databse/required-data.sql
This has the required game, users, groups with resources etc but note that this does not include the admin account so it is not a replacement for the actual sugar databse, it is an addition/insertion to be added once the initial database has been setup.
