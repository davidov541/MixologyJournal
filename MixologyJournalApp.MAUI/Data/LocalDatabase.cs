using MixologyJournalApp.MAUI.Model;
using SQLite;

namespace MixologyJournalApp.MAUI.Data;

internal class LocalDatabase
{
    private const string DatabaseFilename = "MJSQLite.db3";

    private const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    private static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    private SQLiteAsyncConnection _database = null;

    internal LocalDatabase()
    {
    }

    private async Task Init()
    {
        if (this._database is null)
        {
            this._database = new SQLiteAsyncConnection(DatabasePath, Flags);
            await this.SetupInitialValues(InitialModels.Units);
            await this.SetupInitialValues(InitialModels.Ingredients);
            await this.SetupInitialValues(InitialModels.Recipes);
        }
    }

    private async Task SetupInitialValues<T>(List<T> initialValues) where T: new()
    {
        CreateTableResult result = await this._database.CreateTableAsync<T>();
        foreach (T value in initialValues)
        {
            await this._database.InsertOrReplaceAsync(value);
        }
    }

    internal async Task<List<T>> GetItemsAsync<T>() where T : new()
    {
        await Init();
        return await this._database.Table<T>().ToListAsync();
    }

    //public async Task<List<TodoItem>> GetItemsNotDoneAsync()
    //{
    //    await Init();
    //    return await Database.Table<TodoItem>().Where(t => t.Done).ToListAsync();

    //    // SQL queries are also possible
    //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    //}

    //public async Task<TodoItem> GetItemAsync(int id)
    //{
    //    await Init();
    //    return await Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    //}

    //public async Task<int> SaveItemAsync(TodoItem item)
    //{
    //    await Init();
    //    if (item.ID != 0)
    //    {
    //        return await Database.UpdateAsync(item);
    //    }
    //    else
    //    {
    //        return await Database.InsertAsync(item);
    //    }
    //}

    //public async Task<int> DeleteItemAsync(TodoItem item)
    //{
    //    await Init();
    //    return await Database.DeleteAsync(item);
    //}
}