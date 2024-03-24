using System;
using Godot;
using Vain.Core;

namespace Vain.HubSystem.Query;


public interface IQuery<Rq,Rs>
    where Rq : IRequest
    where Rs : IQueryResponse {}
public interface IRequest {}
public interface IQueryResponse {}
public interface IDataProvider {}

public interface IDataProvider<Q,Rq,Rs> : IDataProvider
    where Q : IQuery<Rq,Rs>
    where Rq : IRequest
    where Rs  : struct, IQueryResponse
    {
    Rs? Provide (Rq request);

}

public class DataProviderNotFoundException<Q,Rq,Rs> : Exception
    where Q : IQuery<Rq,Rs>
    where Rq : IRequest
    where Rs : IQueryResponse
{
    public DataProviderNotFoundException() : base($"Could not find data provider for query {typeof(Q).Name}")
    {}
}