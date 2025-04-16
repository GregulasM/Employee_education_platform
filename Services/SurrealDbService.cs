using SurrealDb.Net;
using SurrealDb.Net.Models;
using SurrealDb.Net.Models.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_education_platform.Services;

public class SurrealDbService
{
    private readonly SurrealDbClient _db;

    public SurrealDbService(SurrealDbClient db)
    {
        _db = db;
        _db.SignIn(new RootAuth { Username = "root", Password = "root" }).Wait();
        _db.Use("test", "test").Wait();
    }

    public async Task<T> CreateRecord<T>(string table, T record)
    {
        var response = await _db.Create(table, record);
        return response;
    }

    public async Task<IEnumerable<T>> SelectAll<T>(string table)
    {
        var response = await _db.Select<T>(table);
        return response ?? Enumerable.Empty<T>();
    }

    public async Task<T> SelectById<T>(string table, string id)
    {
        var recordId = $"{table}:{id}";
        var response = await _db.Select<T>(recordId);
        return response.FirstOrDefault();
    }

    public async Task<TOutput> Merge<TMerge, TOutput>(TMerge record) where TMerge : Record
    {
        var response = await _db.Merge<TMerge, TOutput>(record);
        return response;
    }

    public async Task Delete(string table, string id)
    {
        var recordId = $"{table}:{id}";
        await _db.Delete(recordId);
    }

    public async Task<List<T>> RunQuery<T>(string query, Dictionary<string, object> parameters)
    {
        
        var response = await _db.Query(query, parameters);
        return response.GetValue<List<T>>(0) ?? new List<T>();
        
    }

}