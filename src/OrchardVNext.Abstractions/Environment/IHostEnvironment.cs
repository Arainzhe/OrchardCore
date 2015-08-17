﻿namespace OrchardVNext.Abstractions.Environment {
    /// <summary>
    /// Abstraction of the running environment
    /// </summary>
    public interface IHostEnvironment {
        string MapPath(string virtualPath);
    }
}