using System;

namespace cursework.Models;

public class Result<T_OK, T_ERR>
{
    private T_OK? Ok { get; set; }
    private T_ERR? Err { get; set; }
    public bool IsOk { get; private set; }
    public bool IsErr { get; private set; }

    protected Result(T_OK ok, T_ERR err, bool isOk, bool isErr)
    {
        this.Ok = ok;
        this.Err = err;
        this.IsOk = isOk;
        this.IsErr = isErr;
    }
    
    public static Result<T_OK, T_ERR> Success(T_OK? ok = default)
    {
        return new Result<T_OK, T_ERR>(ok, default(T_ERR), true, false);
    }
    public static Result<T_OK, T_ERR> Failure(T_ERR? err = default)
    {
        return new Result<T_OK, T_ERR>(default(T_OK), err, false, true);
    }
    
    public T_OK Unwrap()
    {
        if (IsOk)
        {
            return Ok;
        }
        else
        {
            throw new InvalidOperationException($"Cannot unwrap an error result of Err: {this.Err}");
        }
    }
}

public class Result : Result<object?, object?>
{
    private Result(object? ok, object? err, bool isOk, bool isErr) : base(ok, err, isOk, isErr) { }
    
    public new static Result Success(object? ok = null)
    {
        return new Result(ok, null, true, false);
    }
    public new static Result Failure(object? err = null)
    {
        return new Result(null, err, false, true);
    }
}