# EF Core Migrations vs Manual Migration

I think of EF Core migrations as the smart and organized way. I update my model, run `Add-Migration`, and then `Update-Database`, and EF creates the database changes for me. It keeps a history of changes and makes it easier to move the project forward without losing track of what changed.

Manual migration, like I did in this project, is more hands-on. I had to create or update the database schema myself, usually with SQL or direct changes, and there was no automatic tracking from EF. That gives more control, but it is easier to forget a change or mismatch the database with the code.

So basically: EF Core migrations are automated and versioned, while manual migration is more manual and depends on me being careful every step of the way.
