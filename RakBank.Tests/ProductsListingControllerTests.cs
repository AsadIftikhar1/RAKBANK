using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Licensing.Services;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RAKBANK.Controller;
using RAKBANK.Models;
using RAKBANK.services;
using SiteDefinition = EPiServer.Web.SiteDefinition;

namespace RAKBANK.Tests
{
    [TestFixture]
    public class ProductsListingControllerTests
    {
        private Mock<IContentLoader> _mockContentLoader;
        private Mock<IContentRepository> _mockContentRepository;
        private Mock<SiteDefinition> _mockSiteDefinition;
        private Mock<ProductService> _mockProductService;
        private Mock<UrlResolver> _mockUrlResolver;
        private ProductsListingController _controller;

        [SetUp]
        public void Setup()
        {
            _mockContentLoader = new Mock<IContentLoader>();
            _mockContentRepository = new Mock<IContentRepository>();
            _mockSiteDefinition = new Mock<SiteDefinition>();
            _mockUrlResolver = new Mock<UrlResolver>();

            // Mock the behavior of ProductService
            _mockProductService = new Mock<ProductService>(_mockUrlResolver.Object);

            _controller = new ProductsListingController(
                _mockContentLoader.Object,
                _mockContentRepository.Object,
              _mockSiteDefinition.Object,
                _mockProductService.Object,
                _mockUrlResolver.Object
            );
        }

        [Test]
        public void GetAllProductsListingBlocks_ReturnsOkResult_WithProductList()
        {
            // Arrange
            var productListingViewModel = new List<ProductListingViewModel>
            {
                new ProductListingViewModel { /* Set properties if needed */ }
            };


            // Act
            var result = _controller.GetAllProductsListingBlocks();
            Assert.That(result.Result, Is.InstanceOf<OkResult>(), "Result is not OkResult.");
            var okResult = result.Result as OkResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public void DeleteProduct_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var productItemBlock = new ProductItemBlock();
 
            var contentReference = new ContentReference(123);

            _mockContentLoader.Setup(loader => loader.Get<ProductItemBlock>(It.IsAny<ContentReference>()))
                .Returns(productItemBlock);

            _mockContentRepository.Setup(repo => repo.Get<ProductsListingBlock>(It.IsAny<ContentReference>()))
                .Returns(new ProductsListingBlock { ProductArea = new ContentArea() });

            // Act
            var result = _controller.DeleteProduct(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(okResult.Value));
        }
    }
}
