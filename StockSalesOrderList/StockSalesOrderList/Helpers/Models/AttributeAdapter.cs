using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList
{
    public class AttributeAdapter
    {
        public static void Register()
        {
            DefaultModelBinder.ResourceClassKey = "Messages";
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(CustomRequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinLengthAttribute), typeof(CustomMinLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxLengthAttribute), typeof(CustomMaxLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(System.ComponentModel.DataAnnotations.CompareAttribute), typeof(CustomCompareAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(CustomEmailAddressAttributeAdapter));
        }
    }
    public class CustomRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public CustomRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(App_GlobalResources.Messages);
            attribute.ErrorMessageResourceName = "ValidateRequired";
            attribute.ErrorMessage = null;
        }
    }

    public class CustomMaxLengthAttributeAdapter : MaxLengthAttributeAdapter
    {
        public CustomMaxLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, MaxLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(App_GlobalResources.Messages);
            attribute.ErrorMessageResourceName = "ValidateMaxLength";
            attribute.ErrorMessage = null;
        }
    }

    public class CustomMinLengthAttributeAdapter : MinLengthAttributeAdapter
    {
        public CustomMinLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, MinLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(App_GlobalResources.Messages);
            attribute.ErrorMessageResourceName = "ValidateMinLength";
            attribute.ErrorMessage = null;
        }
    }

    public class CustomCompareAttributeAdapter : DataAnnotationsModelValidator<System.ComponentModel.DataAnnotations.CompareAttribute>
    {
        public CustomCompareAttributeAdapter(ModelMetadata metadata, ControllerContext context, System.ComponentModel.DataAnnotations.CompareAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(App_GlobalResources.Messages);
            attribute.ErrorMessageResourceName = "ValidateCompare";
            attribute.ErrorMessage = null;
        }
    }

    public class CustomEmailAddressAttributeAdapter : DataAnnotationsModelValidator<EmailAddressAttribute>
    {
        public CustomEmailAddressAttributeAdapter(ModelMetadata metadata, ControllerContext context, EmailAddressAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(App_GlobalResources.Messages);
            attribute.ErrorMessageResourceName = "ValidateEmail";
            attribute.ErrorMessage = null;
        }
    }
}
