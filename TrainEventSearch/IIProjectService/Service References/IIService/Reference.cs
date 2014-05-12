﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IIProjectService.IIService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="IIService.INamingService")]
    public interface INamingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INamingService/GetVehicle", ReplyAction="http://tempuri.org/INamingService/GetVehicleResponse")]
        System.Xml.Linq.XElement GetVehicle(string epc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INamingService/GetLocation", ReplyAction="http://tempuri.org/INamingService/GetLocationResponse")]
        System.Xml.Linq.XElement GetLocation(string epc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INamingService/GetAllLocations", ReplyAction="http://tempuri.org/INamingService/GetAllLocationsResponse")]
        System.Xml.Linq.XElement GetAllLocations();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INamingServiceChannel : IIProjectService.IIService.INamingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NamingServiceClient : System.ServiceModel.ClientBase<IIProjectService.IIService.INamingService>, IIProjectService.IIService.INamingService {
        
        public NamingServiceClient() {
        }
        
        public NamingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NamingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NamingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NamingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Xml.Linq.XElement GetVehicle(string epc) {
            return base.Channel.GetVehicle(epc);
        }
        
        public System.Xml.Linq.XElement GetLocation(string epc) {
            return base.Channel.GetLocation(epc);
        }
        
        public System.Xml.Linq.XElement GetAllLocations() {
            return base.Channel.GetAllLocations();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="IIService.IEpcisEventService")]
    public interface IEpcisEventService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEpcisEventService/GetFilenames", ReplyAction="http://tempuri.org/IEpcisEventService/GetFilenamesResponse")]
        string[] GetFilenames();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEpcisEventService/GetEvent", ReplyAction="http://tempuri.org/IEpcisEventService/GetEventResponse")]
        System.Xml.Linq.XElement GetEvent(string filename);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEpcisEventService/GetEvents", ReplyAction="http://tempuri.org/IEpcisEventService/GetEventsResponse")]
        System.Xml.Linq.XElement[] GetEvents(System.DateTime fromInclusive, System.DateTime toInclusive, string readPointEPC);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEpcisEventServiceChannel : IIProjectService.IIService.IEpcisEventService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EpcisEventServiceClient : System.ServiceModel.ClientBase<IIProjectService.IIService.IEpcisEventService>, IIProjectService.IIService.IEpcisEventService {
        
        public EpcisEventServiceClient() {
        }
        
        public EpcisEventServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EpcisEventServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EpcisEventServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EpcisEventServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] GetFilenames() {
            return base.Channel.GetFilenames();
        }
        
        public System.Xml.Linq.XElement GetEvent(string filename) {
            return base.Channel.GetEvent(filename);
        }
        
        public System.Xml.Linq.XElement[] GetEvents(System.DateTime fromInclusive, System.DateTime toInclusive, string readPointEPC) {
            return base.Channel.GetEvents(fromInclusive, toInclusive, readPointEPC);
        }
    }
}
