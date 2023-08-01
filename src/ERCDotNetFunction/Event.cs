using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ERCDotNetFunction
{

    public class Payload
    {
        public string? LoanId { get; set; }
        public string? AppUserId { get; set; }
        public string? XGrCompany { get; set; }
        public string? EventName { get; set; }
        public List<string>? Borrowers { get; set; }
        public List<string>? CoBorrowers { get; set; }
    }

    public class Event
    {
        [JsonProperty(PropertyName = "version")]
        public string? Version { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        [JsonProperty(PropertyName = "detail-type")]
        public string? DetailType { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string? Source { get; set; }
         [JsonProperty(PropertyName = "account")]
        public string? Account { get; set; }
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }
        
        [JsonProperty(PropertyName = "region")]
        public string? Region { get; set; }
        
        [JsonProperty(PropertyName = "detail")]
        public Detail? Detail { get; set; }
    }

    public class Detail
    {
        
        [JsonProperty(PropertyName = "messageId")]
        public string? MessageId { get; set; }

        [JsonProperty(PropertyName = "receiptHandle")]
        public string? ReceiptHandle { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string? Body { get; set; }
        public Body? JsonEventBody { get; set; }
        
        [JsonProperty(PropertyName = "Timestamp")]
        public DateTime? Timestamp { get; set; }
        
        [JsonProperty(PropertyName = "eventSource")]
        public string? EventSource { get; set; }
        
        [JsonProperty(PropertyName = "eventSourceARN")]
        public string? EventSourceARN { get; set; }
        
        [JsonProperty(PropertyName = "awsRegion")]
        public string? AwsRegion { get; set; }
    }

    public class Body
    {
        public string? Type { get; set; }
        public string? MessageId { get; set; }
        public string? TopicArn { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public Message? JsonEventMessage { get; set; }
    }


    public class Message
    {
        [JsonProperty(PropertyName = "triggered")]
        public bool Triggered { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }
        [JsonProperty(PropertyName = "loan-number")]
        public string? LoanNumber { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public Msg? Msg { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string? Source { get; set; }

        [JsonProperty(PropertyName = "env")]
        public string? Env { get; set; }

        [JsonProperty(PropertyName = "loan-version")]
        public int LoanVersion { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "loan-guid")]
        public string? LoanGuid { get; set; }
    }

    public class Msg
    {
        [JsonProperty(PropertyName = "event-header")]
        public EventHeader? EventHeader { get; set; }

        [JsonProperty(PropertyName = "details")]
        public List<DetailItem> Details { get; set; }
    }

    public class EventHeader
    {
        [JsonProperty(PropertyName = "partner")]
        public string? Partner { get; set; }

        [JsonProperty(PropertyName = "encompass-lastmodified")]
        public DateTime EncompassLastModified { get; set; }

        [JsonProperty(PropertyName = "loan-number")]
        public string? LoanNumber { get; set; }

        [JsonProperty(PropertyName = "loan-tenant")]
        public string? LoanTenant { get; set; }

        [JsonProperty(PropertyName = "user-id")]
        public string? UserId { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string? Source { get; set; }

        [JsonProperty(PropertyName = "env")]
        public string? Env { get; set; }

        [JsonProperty(PropertyName = "borr-last-name")]
        public string? BorrLastName { get; set; }

        [JsonProperty(PropertyName = "borrower-pairs")]
        public List<BorrowerPair>? BorrowerPairs { get; set; }


        [JsonProperty(PropertyName = "urla-version")]
        public string? UrlaVersion { get; set; }

        [JsonProperty(PropertyName = "loan-owner")]
        public string? LoanOwner { get; set; }

        [JsonProperty(PropertyName = "team")]
        public List<Team>? Team { get; set; }

        [JsonProperty(PropertyName = "loan-guid")]
        public string? LoanGuid { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
        [JsonProperty(PropertyName = "prev-version")]
        public int PrevVersion { get; set; }
        [JsonProperty(PropertyName = "company")]
        public string? Company { get; set; }
    }

    public class BorrowerPair
    {
        [JsonProperty(PropertyName = "borr-first-name")]
        public string? BorrFirstName { get; set; }

        [JsonProperty(PropertyName = "borr-last-name")]
        public string? BorrLastName { get; set; }

        [JsonProperty(PropertyName = "borr-email")]
        public string? BorrEmail { get; set; }

        [JsonProperty(PropertyName = "co-borr-first-name")]
        public string? CoBorrFirstName { get; set; }


        [JsonProperty(PropertyName = "co-borr-last-name")]
        public string? CoBorrLastName { get; set; }



        [JsonProperty(PropertyName = "co-borr-email")]
        public string? CoBorrEmail { get; set; }
    }

    public class Team
    {


        [JsonProperty(PropertyName = "role")]
        public string? Role { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public object? UserId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public object? Email { get; set; }
    }

    public class DetailItem
    {
        [JsonProperty(PropertyName = "entitiy-type")]
        public string EntitiyType { get; set; }

        [JsonProperty(PropertyName = "entity-email")]
        public string EntityEmail { get; set; }
    }

    public class RequestBody
    {
        [JsonProperty(PropertyName = "task-title")]
        public string? TaskTitle { get; set; }

        [JsonProperty(PropertyName = "task-description")]
        public string? TaskDescription { get; set; }

        [JsonProperty(PropertyName = "task-key")]
        public string? TaskKey { get; set; }

        [JsonProperty(PropertyName = "task-action-type")]
        public string? TaskActionType { get; set; }

        [JsonProperty(PropertyName = "task-display-type")]
        public string? TaskDisplayType { get; set; }

        [JsonProperty(PropertyName = "task-type")]
        public string? TaskType { get; set; }

        [JsonProperty(PropertyName = "task-category")]
        public string? TaskCategory { get; set; }

        [JsonProperty(PropertyName = "task-template-id")]
        public string? TaskTemplateId { get; set; }

        [JsonProperty(PropertyName = "borrowers")]
        public List<Borrower>? Borrowers { get; set; }

        [JsonProperty(PropertyName = "app-user-id")]
        public string? AppUserId { get; set; }
    }

    public class Explanation
    {
        [JsonProperty(PropertyName = "explanationtype")]
        public string? ExplanationType { get; set; }
    }

    public class Borrower
    {
        [JsonProperty(PropertyName = "emailAddress")]
        public string? EmailAddress { get; set; }
    }

    public class ClientSecrets
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class TaskMasterResponse
    {
        [JsonProperty(PropertyName = "task-id")]
        public string? TaskId { get; set; }
        [JsonProperty(PropertyName = "case-id")]
        public string? CaseId { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string? Source { get; set; }
        [JsonProperty(PropertyName = "domain-entity-id")]
        public object? DomainEntityId { get; set; }
        [JsonProperty(PropertyName = "task-tags")]
        public object? TaskTags { get; set; }
        [JsonProperty(PropertyName = "deleted")]
        public bool? Deleted { get; set; }
        [JsonProperty(PropertyName = "task-owner")]
        public object? TaskOwner { get; set; }
        [JsonProperty(PropertyName = "request-id")]
        public object? RequestId { get; set; }
        [JsonProperty(PropertyName = "task-strategy")]
        public object? TaskStrategy { get; set; }
        [JsonProperty(PropertyName = "task-key")]
        public string? TaskKey { get; set; }
        [JsonProperty(PropertyName = "created-on")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty(PropertyName = "task-prior-to-date")]
        public object? TaskPriorToDate { get; set; }
        [JsonProperty(PropertyName = "task-action-type")]
        public string? TaskActionType { get; set; }
        [JsonProperty(PropertyName ="task-sync-id")]
        public object? TaskSyncId { get; set; }
        [JsonProperty(PropertyName = "updated-on")]
        public object? UpdatedOn { get; set; }
        [JsonProperty(PropertyName = "task-category")]
        public string? TaskCategory { get; set; }
        [JsonProperty(PropertyName = "task-prior-to-milestone")]
        public object? TaskPriorToMilestone { get; set; }
        [JsonProperty(PropertyName = "task-assigned-to")]
        public object? TaskAssignedTo { get; set; }
        [JsonProperty(PropertyName = "task-completed")]
        public bool TaskCompleted { get; set; }
        [JsonProperty(PropertyName = "task-type")]
        public object? DomainEntityType { get; set; }
        [JsonProperty(PropertyName = "consumer-id")]
        public object? ConsumerId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public object? Domain { get; set; }
        [JsonProperty(PropertyName = "tenant-id")]
        public object? TenantId { get; set; }
        [JsonProperty(PropertyName = "task-description")]
        public string? TaskDescription { get; set; }
        [JsonProperty(PropertyName = "task-title")]
        public string? TaskTitle { get; set; }
    }

    public class TaskMasterResult
    {
        public TaskMasterResponse? BorrowerResponse { get; set; }
        public TaskMasterResponse? CoBorrowerResponse { get; set; }
    }
}