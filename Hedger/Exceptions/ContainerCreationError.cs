using System;

namespace Hedger.Exceptions;
public class ContainerCreationError(string message) : Exception(message) {
}
