using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUTest
{
    public class ModelStateControllerTests : Controller
    {
        public ModelStateControllerTests()
        {
            ControllerContext = (new Mock<ControllerContext>()).Object;
        }
    }
}
