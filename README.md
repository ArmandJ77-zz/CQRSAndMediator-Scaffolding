# CQRS and Mediator Scaffolding

<img alt="Nuget" src="https://img.shields.io/nuget/v/CQRSAndMediator.Scaffolding?logo=CQRSAndMediator.Scaffolding&style=for-the-badge">

## Medium article

> [CQRS & Mediator Part 2: Domain scaffolding with Roslyn API and Dotnet CLI](https://medium.com/@armandjordaan6/cqrs-mediator-part-2-domain-scaffolding-with-roslyn-api-and-dotnet-cli-7c99b5b011f) in this part we will be building a dotnet CLI tool which follows the CQRS and Mediator patterns to auto generate commands, queries, responses and handlers in the domain layer using Roslyn API for code generation.

## Breakdown of concepts and commands

Show help information:

```
scaffold -h
```

#### Command Parameters:

**Concern:** Name of the domain area you are working in i.e Orders, People, Invoice ect.

```
-c|--concern <NAME>
```

**Operation:** Name of the action you are taking in your concern i.e GetById, GetPagedResult for queries or Create, Patch, Update for commands.

```
-c|--concern <NAME>
```

**OperationType:** The type of CQRS operation you are scaffolding i.e command or query

```
-ot|--operationtype <TYPE>
```

**GroupBy:** Specify how to group the domain actions. Either by _concern_ or _operation_:
Group domain objects by [C] for concerns or [O] for operations, **defaults to concerns**

```
-g|--groupBy <TYPE>
```

By **concern**:
i.e group by concern when parameters are -c Orders -o GetById -ot query

```

| YourDomainLayer
    | Orders
        | Handlers
            | OrdersGetByIdHandler.cs
        | Responses
            | OrdersGetByIdResponse.cs
        | Queries
            | OrderGetByIdQuery.cs

```

By **operation**
i.e group by operation when parameters are -c Orders -o GetById -ot query -g O

```
| YourDomainLayer
    | Handler
        | OrdersGetByIdHandler.cs
    | Responses
        | OrdersGetByIdResponse.cs
    | Queries
        | OrderGetByIdQuery.cs
```

## Installation

Install using dotnet cli:

```
dotnet tool install --global CQRSAndMediator.Scaffolding
```

To uninstall use:

```
dotnet tool uninstall cqrsandmediator.scaffolding --global
```

## Usage

**Note:** The tool requires that the project is setup already and that the actions are executed in the top level directory of where your domain layer directory is located.

Use case scaffold out the CRUD domain for an invoice:

The Create Command

```
scaffold -c Invoices -o Create -ot command
```

The Get By id query

```
scaffold -c Invoices -o GetById -ot query
```

The Patch Command

```
scaffold -c Invoices -o Patch -ot command
```

## Develop locally
Update the nuget package version located in CQRSAndMediator.Scaffolding.csproj

```
dotnet build .
dotnet pack
```

then in the solution directory:
```
dotnet tool update -g cqrsandmediator.scaffolding --add-source ./nupkg --version x.x.x
```

