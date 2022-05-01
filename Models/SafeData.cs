using System;
using System.Collections.Generic;

namespace gnosis_safe_signer_report.Models
{
    public class SafeData
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<TransactionInfo> results { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }
        public List<ValueDecoded> valueDecoded { get; set; }
    }

    public class DataDecoded
    {
        public string method { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class ValueDecoded
    {
        public string data { get; set; }
        public DataDecoded dataDecoded { get; set; }
        public int operation { get; set; }
        public string to { get; set; }
        public string value { get; set; }
    }

    public class Confirmation
    {
        public string owner { get; set; }
        public string signature { get; set; }
        public string signatureType { get; set; }
        public DateTime submissionDate { get; set; }
        public object transactionHash { get; set; }
    }

    public class TokenInfo
    {   
        public string address { get; set; }
        public int? decimals { get; set; }
        public string logoUri { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }
    }

    public class Transfer
    {
        public int blockNumber { get; set; }
        public DateTime executionDate { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string tokenAddress { get; set; }
        public string tokenId { get; set; }
        public TokenInfo tokenInfo { get; set; }
        public string transactionHash { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class TransactionInfo
    {
        public int baseGas { get; set; }
        public int? blockNumber { get; set; }
        public List<Confirmation> confirmations { get; set; }
        public int? confirmationsRequired { get; set; }
        public string data { get; set; }
        public DataDecoded dataDecoded { get; set; }
        public string ethGasPrice { get; set; }
        public DateTime? executionDate { get; set; }
        public string executor { get; set; }
        public string fee { get; set; }
        public string from { get; set; }
        public string gasPrice { get; set; }
        public string gasToken { get; set; }
        public int? gasUsed { get; set; }
        public bool isExecuted { get; set; }
        public bool? isSuccessful { get; set; }
        public string maxFeePerGas { get; set; }
        public string maxPriorityFeePerGas { get; set; }
        public DateTime modified { get; set; }
        public int nonce { get; set; }
        public int operation { get; set; }
        public string origin { get; set; }
        public string refundReceiver { get; set; }
        public string safe { get; set; }
        public int safeTxGas { get; set; }
        public string safeTxHash { get; set; }
        public string signatures { get; set; }
        public DateTime submissionDate { get; set; }
        public string to { get; set; }
        public string transactionHash { get; set; }
        public List<Transfer> transfers { get; set; }
        public bool trusted { get; set; }
        public string txType { get; set; }
        public string txHash { get; set; }
        public string value { get; set; }
    }
}
