using SpyStore.DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xunit;

namespace SpyStore.DAL.Tests
{
    public static class InvalidQuantityExceptionTests
    {

        [Fact]
        public static void InvalidQuantityException_default_ctor()
        {
            // Arrange
            const string expectedMessage = "Exception of type 'SpyStore.DAL.Exceptions.InvalidQuantityException' was thrown.";

            // Act
            var sut = new InvalidQuantityException();

            // Assert
            Assert.Null(sut.ResourceReferenceProperty);
            Assert.Null(sut.InnerException);
            Assert.Equal(expectedMessage, sut.Message);
        }


        [Fact]
        public static void  InvalidQuantityException_ctor_string()
        {
            // Arrange
            const string expectedMessage = "message";

            // Act
            var sut = new  InvalidQuantityException(expectedMessage);

            // Assert
            Assert.Null(sut.ResourceReferenceProperty);
            Assert.Null(sut.InnerException);
            Assert.Equal(expectedMessage, sut.Message);
        }

        [Fact]
        public static void  InvalidQuantityException_ctor_string_ex()
        {
            // Arrange
            const string expectedMessage = "message";
            var innerEx = new Exception("foo");

            // Act
            var sut = new  InvalidQuantityException(expectedMessage, innerEx);

            // Assert
            Assert.Null(sut.ResourceReferenceProperty);
            Assert.Equal(innerEx, sut.InnerException);
            Assert.Equal(expectedMessage, sut.Message);
        }

        [Fact]
        public static void  InvalidQuantityException_serialization_deserialization_test()
        {
            // Arrange
            var innerEx = new Exception("foo");
            var originalException = new  InvalidQuantityException("message", innerEx) { ResourceReferenceProperty = "MyReferenceProperty" };
            var buffer = new byte[4096];
            var ms = new MemoryStream(buffer);
            var ms2 = new MemoryStream(buffer);
            var formatter = new BinaryFormatter();

            // Act
            formatter.Serialize(ms, originalException);
            var deserializedException = ( InvalidQuantityException)formatter.Deserialize(ms2);

            // Assert
            Assert.Equal(originalException.ResourceReferenceProperty, deserializedException.ResourceReferenceProperty);
            Assert.Equal(originalException.InnerException.Message, deserializedException.InnerException.Message);
            Assert.Equal(originalException.Message, deserializedException.Message);
        }

    }
}
