using Dapper;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class ProviderProfileRepository : IProviderProfileRepository
    {
        private readonly string _connStr;

        public ProviderProfileRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private IDbConnection Conn() => new SqlConnection(_connStr);

        // ──────────────────────────────────────────────────────
        // Profile (tbl_Providers + ProviderProfile + login email)
        // ──────────────────────────────────────────────────────
        public async Task<ProviderProfileVM?> GetProfileAsync(int providerId)
        {
            const string sql = @"
                SELECT
                    p.ProviderId, p.FirmName, p.ProviderName, p.Phone,
                    (SELECT TOP 1 u.Email FROM Tbl_Users u
                      WHERE u.ProviderId = p.ProviderId AND u.RoleId = 3
                      ORDER BY u.UserId) AS Email,
                    pp.LegalStatus, pp.RegistrationNo, pp.RegistrationAuthority,
                    pp.GSTNo, pp.PANNo, pp.EPFNo, pp.ESINo, pp.MSMENo, pp.Address,
                    pp.ContactDesignation,
                    pp.ContractManagerName, pp.ContractManagerPhone, pp.ContractManagerExpYears,
                    pp.BankAccountName, pp.BankAccountNo, pp.BankIFSC, pp.BankName, pp.BankBranch,
                    pp.UpdatedOn
                FROM tbl_Providers p
                LEFT JOIN ProviderProfile pp ON pp.ProviderId = p.ProviderId
                WHERE p.ProviderId = @providerId";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<ProviderProfileVM>(sql, new { providerId });
        }

        public async Task SaveProfileAsync(ProviderProfileVM m, int userId)
        {
            using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            await conn.ExecuteAsync(@"
                UPDATE tbl_Providers
                SET FirmName = @FirmName, ProviderName = @ProviderName, Phone = @Phone
                WHERE ProviderId = @ProviderId",
                new { m.FirmName, m.ProviderName, m.Phone, m.ProviderId }, tran);

            await conn.ExecuteAsync(@"
                MERGE ProviderProfile AS t
                USING (SELECT @ProviderId AS ProviderId) AS s
                   ON t.ProviderId = s.ProviderId
                WHEN MATCHED THEN UPDATE SET
                    LegalStatus = @LegalStatus, RegistrationNo = @RegistrationNo,
                    RegistrationAuthority = @RegistrationAuthority,
                    GSTNo = @GSTNo, PANNo = @PANNo, EPFNo = @EPFNo, ESINo = @ESINo,
                    MSMENo = @MSMENo, Address = @Address, ContactDesignation = @ContactDesignation,
                    ContractManagerName = @ContractManagerName,
                    ContractManagerPhone = @ContractManagerPhone,
                    ContractManagerExpYears = @ContractManagerExpYears,
                    BankAccountName = @BankAccountName, BankAccountNo = @BankAccountNo,
                    BankIFSC = @BankIFSC, BankName = @BankName, BankBranch = @BankBranch,
                    UpdatedOn = GETDATE(), UpdatedBy = @UserId
                WHEN NOT MATCHED THEN INSERT
                    (ProviderId, LegalStatus, RegistrationNo, RegistrationAuthority,
                     GSTNo, PANNo, EPFNo, ESINo, MSMENo, Address, ContactDesignation,
                     ContractManagerName, ContractManagerPhone, ContractManagerExpYears,
                     BankAccountName, BankAccountNo, BankIFSC, BankName, BankBranch,
                     UpdatedOn, UpdatedBy)
                VALUES
                    (@ProviderId, @LegalStatus, @RegistrationNo, @RegistrationAuthority,
                     @GSTNo, @PANNo, @EPFNo, @ESINo, @MSMENo, @Address, @ContactDesignation,
                     @ContractManagerName, @ContractManagerPhone, @ContractManagerExpYears,
                     @BankAccountName, @BankAccountNo, @BankIFSC, @BankName, @BankBranch,
                     GETDATE(), @UserId);",
                new
                {
                    m.ProviderId, m.LegalStatus, m.RegistrationNo, m.RegistrationAuthority,
                    m.GSTNo, m.PANNo, m.EPFNo, m.ESINo, m.MSMENo, m.Address, m.ContactDesignation,
                    m.ContractManagerName, m.ContractManagerPhone, m.ContractManagerExpYears,
                    m.BankAccountName, m.BankAccountNo, m.BankIFSC, m.BankName, m.BankBranch,
                    UserId = userId
                }, tran);

            tran.Commit();
        }

        // ──────────────────────────────────────────────────────
        // Documents
        // ──────────────────────────────────────────────────────
        public async Task<List<ProviderDocumentVM>> GetDocumentsAsync(int providerId)
        {
            const string sql = @"
                SELECT Id, ProviderId, DocumentType, DocumentNo, ValidTill, FileName,
                       OriginalFileName, ContentType, FileSize, UploadedOn
                FROM ProviderDocuments
                WHERE ProviderId = @providerId AND IsDeleted = 0
                ORDER BY UploadedOn DESC";

            using var conn = Conn();
            return (await conn.QueryAsync<ProviderDocumentVM>(sql, new { providerId })).ToList();
        }

        public async Task<ProviderDocumentVM?> GetDocumentAsync(int documentId)
        {
            const string sql = @"
                SELECT Id, ProviderId, DocumentType, DocumentNo, ValidTill, FileName,
                       OriginalFileName, ContentType, FileSize, UploadedOn
                FROM ProviderDocuments
                WHERE Id = @documentId AND IsDeleted = 0";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<ProviderDocumentVM>(sql, new { documentId });
        }

        public async Task<int> AddDocumentAsync(ProviderDocumentVM d, int userId)
        {
            const string sql = @"
                INSERT INTO ProviderDocuments
                    (ProviderId, DocumentType, DocumentNo, ValidTill, FileName,
                     OriginalFileName, ContentType, FileSize, UploadedOn, UploadedBy)
                VALUES
                    (@ProviderId, @DocumentType, @DocumentNo, @ValidTill, @FileName,
                     @OriginalFileName, @ContentType, @FileSize, GETDATE(), @UserId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = Conn();
            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                d.ProviderId, d.DocumentType, d.DocumentNo, d.ValidTill, d.FileName,
                d.OriginalFileName, d.ContentType, d.FileSize, UserId = userId
            });
        }

        // Soft delete — file disk pe rehti hai (audit ke liye)
        public async Task<bool> DeleteDocumentAsync(int documentId, int providerId)
        {
            const string sql = @"
                UPDATE ProviderDocuments SET IsDeleted = 1
                WHERE Id = @documentId AND ProviderId = @providerId AND IsDeleted = 0";

            using var conn = Conn();
            return await conn.ExecuteAsync(sql, new { documentId, providerId }) > 0;
        }
    }
}
