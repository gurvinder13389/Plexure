using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    public class Test
    {
        Service service;

        [OneTimeSetUp]
        public void Setup()
        {
            service = new Service();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }

        [Test]
        public void TEST_001_1_Purchase_Product_Message_PointCheck()
        {
            int productId = 1;
            var response = service.Purchase(productId);

            response.Expect(HttpStatusCode.Accepted);
            response.ExpectPurchaseMessage("Purchase completed.");
            response.ExpectPurchasePoints(100);

        }

        [Test]
        public void TEST_001_2_Purchase_InvalidProduct()
        {
            int productId = 2;
            var response = service.Purchase(productId);
            String strExpectedErrorMessage = "Invalid product id";

            response.Expect(HttpStatusCode.BadRequest);
            response.ExpectError(strExpectedErrorMessage);
        }

        [Test]
        public void TEST_002_Get_Points_For_Logged_In_User()
        {
            var points = service.GetPoints();
            points.Expect(HttpStatusCode.Accepted);
        }


        [Test]
        public void TEST_003_Login_Unsuccessful_Credentials_Missed()
        {
            var response = service.Login("", "");
            String strExpectedErrorMessage = "Username and password required.";

            response.Expect(HttpStatusCode.BadRequest);
            response.ExpectError(strExpectedErrorMessage);

        }

        [Test]
        public void TEST_003_Login_Unsuccessful_Credentials_NotMatched()
        {
            var response = service.Login("Testar", "Plexure123");

            String strExpectedErrorMessage = "Unauthorized";

            response.Expect(HttpStatusCode.Unauthorized);
            response.ExpectError(strExpectedErrorMessage);
        }


        [Test]
        public void TEST_003_Login_With_Valid_User()
        {
            var response = service.Login("Tester", "Plexure123");

            response.Expect(HttpStatusCode.OK);

        }


    }
}
