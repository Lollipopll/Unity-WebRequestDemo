using System;

[Serializable]
public class BaseResponse<T>
{
    public int code;

    public string message;

    public T data;
}