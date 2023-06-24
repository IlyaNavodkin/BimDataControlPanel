using BimDataControlPanel.DAL.Exeptions;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Extensions;

public static class ControllerExtension
{
    public static async Task GenerateModelStateErrors(this Controller controller,Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            foreach (var validationResultError in validationException.Errors)
            {
                controller.ModelState.AddModelError(validationResultError.PropertyName, validationResultError.ErrorMessage);
            }
        }
        if (exception is NotFoundEntityException notFoundEntityException)
        {
            controller.ModelState.AddModelError(nameof(notFoundEntityException), notFoundEntityException.Message);
        }
        if (exception is EntityDuplicateException entityDuplicateException)
        {
            controller.ModelState.AddModelError(nameof(entityDuplicateException), entityDuplicateException.Message);
        }
        if (exception is EntityMultiDuplicateException entityMultiDuplicateException)
        {
            controller.ModelState.AddModelError(nameof(entityMultiDuplicateException), entityMultiDuplicateException.Message);
        }
        if (exception is NotFoundPropertyException notFoundPropertyException)
        {
            controller.ModelState.AddModelError(nameof(notFoundPropertyException), notFoundPropertyException.Message);
        }
    }
}