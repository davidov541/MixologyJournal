using MixologyJournalApp.MAUI.Model;
using SQLite;
using System.Linq.Expressions;

namespace MixologyJournalApp.MAUI.Data;

internal class LocalDatabase : IStateSaver
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

    private async Task InitAsync()
    {
        if (this._database is null)
        {
            this._database = new SQLiteAsyncConnection(DatabasePath, Flags);
            await this.SetupInitialValuesAsync(InitialModels.Units);
            await this.SetupInitialValuesAsync(InitialModels.Ingredients);
            await this.SetupInitialValuesAsync(InitialModels.Recipes);
        }
    }

    private async Task SetupInitialValuesAsync<T>(List<T> initialValues) where T : ICanSave, new()
    {
        foreach (T value in initialValues)
        {
            await value.SaveAsync(this);
        }
    }

    internal async Task<List<T>> LoadAllModels<T>() where T : new()
    {
        await InitAsync();
        return await this._database.Table<T>().ToListAsync();
    }

    public async Task InsertOrReplaceAsync<T>(T value) where T : new()
    {
        await InitAsync();
        if (!this._database.TableMappings.Where(t => t.TableName == nameof(T)).Any())
        {
            await this._database.CreateTableAsync<T>();
        }
        await this._database.InsertOrReplaceAsync(value);
    }

    public async Task<List<T>> GetFilteredItemsAsync<T>(Expression<Func<T, bool>> condition) where T: new()
    {
        await InitAsync();
        return await this._database.Table<T>().Where(condition).ToListAsync();
    }
}