// Global using directives

global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics.CodeAnalysis;
global using System.Dynamic;
global using System.IO;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.Serialization;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Threading.Tasks;
global using DfT.DTRO.Attributes;
global using DfT.DTRO.Caching;
global using DfT.DTRO.Converters;
global using DfT.DTRO.DAL;
global using DfT.DTRO.Enums;
global using DfT.DTRO.Extensions;
global using DfT.DTRO.Extensions.Configuration;
global using DfT.DTRO.Extensions.DependencyInjection;
global using DfT.DTRO.FeatureManagement;
global using DfT.DTRO.Filters;
global using DfT.DTRO.JsonLogic;
global using DfT.DTRO.Models;
global using DfT.DTRO.Models.DataBase;
global using DfT.DTRO.Models.DtroDtos;
global using DfT.DTRO.Models.DtroEvent;
global using DfT.DTRO.Models.DtroHistory;
global using DfT.DTRO.Models.DtroJson;
global using DfT.DTRO.Models.Errors;
global using DfT.DTRO.Models.Filtering;
global using DfT.DTRO.Models.Metrics;
global using DfT.DTRO.Models.Pagination;
global using DfT.DTRO.Models.RuleTemplate;
global using DfT.DTRO.Models.SchemaTemplate;
global using DfT.DTRO.Models.Search;
global using DfT.DTRO.Models.SharedResponse;
global using DfT.DTRO.Models.SwaCode;
global using DfT.DTRO.RequestCorrelation;
global using DfT.DTRO.Services;
global using DfT.DTRO.Services.Conversion;
global using DfT.DTRO.Services.Mapping;
global using DfT.DTRO.Services.Validation;
global using DfT.DTRO.Utilities;
global using Json.Logic;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.Formatters;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Internal;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Primitives;
global using Microsoft.FeatureManagement;
global using Microsoft.FeatureManagement.Mvc;
global using Microsoft.OpenApi.Models;
global using Npgsql;
global using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions;
global using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
global using NpgsqlTypes;
global using Swashbuckle.AspNetCore.Annotations;
