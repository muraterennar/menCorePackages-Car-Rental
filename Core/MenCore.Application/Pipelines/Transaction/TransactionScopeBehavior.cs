using System.Transactions;
using MediatR;

namespace MenCore.Application.Pipelines.Transaction;

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    #region İstek işlenirken bir işlem kapsamı kullanarak işlemi yöneten metot
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled); // Yeni bir işlem kapsamı oluşturulur ve otomatik olarak yönetilir

        TResponse response; // İşlem sonucu

        try
        {
            response = await next(); // İstek işlenir
            transactionScope.Complete(); // İşlem başarıyla tamamlandığına işaret edilir
        }
        catch
        {
            transactionScope.Dispose(); // İstisna durumunda işlem kapsamı yok edilir
            throw; // İstisna fırlatılır
        }

        return response; // İşlem sonucu döndürülür
    }
    #endregion
}
