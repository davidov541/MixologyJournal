using MixologyJournalApp.MAUI.Model;
using SQLite;
using System.Linq.Expressions;


namespace MixologyJournalApp.MAUI.Data;

internal class LocalDatabase : IStateSaver
{
    private const string DatabaseFilename = "MJSQLite.db3";
    private const string InitialDatabaseFilename = "InitialData.db";

    private const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    private static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    private static string InitialDatabasePath => Path.Combine(FileSystem.CacheDirectory, InitialDatabaseFilename);

    private SQLiteAsyncConnection _database = null;

    internal LocalDatabase()
    {
    }

    private async Task InitAsync()
    {
        if (this._database is null)
        {
            using (Stream copyStream = await FileSystem.OpenAppPackageFileAsync("InitialData.db"))
            {
                using (Stream copyToStream = new FileStream(InitialDatabasePath, FileMode.Create))
                {
                    await copyStream.CopyToAsync(copyToStream);
                }
            }
            this._database = new SQLiteAsyncConnection(DatabasePath, Flags);
            SQLiteAsyncConnection initialDatabase = new SQLiteAsyncConnection(InitialDatabasePath, SQLiteOpenFlags.ReadOnly);
            await this.SetupInitialValuesAsync<Unit>(initialDatabase);
            await this.SetupInitialValuesAsync<Ingredient>(initialDatabase);
            await this.SetupInitialValuesAsync<IngredientUsage>(initialDatabase);
            await this.SetupInitialValuesAsync<Recipe>(initialDatabase);
        }
    }

    private async Task SetupInitialValuesAsync<T>(SQLiteAsyncConnection initialDatabase) where T : ICanSave, new()
    {
        IEnumerable<T> initialValues = await initialDatabase.Table<T>().ToListAsync();
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

    public async Task<List<T>> GetFilteredItemsAsync<T>(Expression<Func<T, bool>> condition) where T : new()
    {
        await InitAsync();
        return await this._database.Table<T>().Where(condition).ToListAsync();
    }
}