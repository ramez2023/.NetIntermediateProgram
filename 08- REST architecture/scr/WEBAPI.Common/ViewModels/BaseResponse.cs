﻿using System.Runtime.Serialization;
using WEBAPI.Common.Enums;

namespace WEBAPI.Common.ViewModels
{
    [DataContract]
    public class BaseResponse
    {
        public BaseResponse()
          : this(0, string.Empty)
        {
        }

        public BaseResponse(int erCode, string erMsg)
            : this(erCode, erMsg, string.Empty)
        {
        }

        public BaseResponse(int erCode, string erMsg, string erDetails)
        {
            ErrorCode = erCode;
            ErrorMsg = erMsg;
            ErrorDetails = erDetails;
            Expiration = new Expiration();
            Persistence = new Persistence();
        }

        public bool IsSucceeded
        {
            get
            {
                return ErrorCode == (int)ErrorCodes.Success;
            }
        }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMsg { get; set; }

        [DataMember]
        public string ErrorDetails { get; set; }

        [DataMember]
        public Expiration Expiration { get; set; }

        [DataMember]
        public Persistence Persistence { get; set; }

        [DataMember]
        public int TotalSeconds { get; set; }


        public override string ToString()
        {
            var json = System.Text.Json.JsonSerializer.Serialize<BaseResponse>(this);
            return json;
        }
    }

    [DataContract]
    public class Expiration
    {
        public Expiration()
        {

        }

        public Expiration(int duration, int method, int mode, bool isSessionExpiry = false)
        {
            IsAllowed = true;
            Duration = duration;
            Method = method;
            Mode = mode;
            IsSessionExpiry = isSessionExpiry;
        }

        [DataMember]
        public bool IsAllowed { get; set; }

        [DataMember]
        public int Duration { get; set; } // In minutes

        [DataMember]
        public int Method { get; set; }

        [DataMember]
        public int Mode { get; set; }

        [DataMember]
        public bool IsSessionExpiry { get; set; }

        public enum ExpiryDuration
        {
            //time in seconds
            NoExpiry = 0,
            QuarterHour = 15 * 60,
            HalfHour = 30 * 60,
            OneHour = 60 * 60,
            OneDay = 24 * 60 * 60
        }
    }

    public class Persistence
    {
        public Persistence()
            : this((int)ScopeLevel.App, false)
        {
        }

        public Persistence(int scope, bool isEncrypted)
        {
            Scope = scope;
            IsEncrypted = isEncrypted;
        }

        [DataMember]
        public int Scope { get; set; }

        [DataMember]
        public bool IsEncrypted { get; set; }

        public enum ScopeLevel
        {
            App = 0,
            User = 1
        }
    }
}