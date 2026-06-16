using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack_API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Registra o erro real no console/log interno da API
            _logger.LogError(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);

            // Prepara uma resposta amigável e segura usando o padrão ProblemDetails (RFC 7807)
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError, // Status 500
                Title = "Erro Interno no Servidor",
                Detail = "Ocorreu um erro inesperado em nosso sistema. Por favor, tente novamente mais tarde.",
                Instance = httpContext.Request.Path
            };

            // Define o status code e o tipo de conteúdo do cabeçalho HTTP
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/json";

            // Escreve o JSON de erro na resposta
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // Retorna true indicando que o erro foi tratado com sucesso e a API não deve quebrar
            return true;
        }
    }
}