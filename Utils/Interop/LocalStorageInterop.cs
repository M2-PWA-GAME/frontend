using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace frontend.Utils.Interop
{
    public class LocalStorageInterop
    {
        public static ValueTask<string> SetItem(IJSRuntime jsRuntime, string key, string value)
        {
            // Implemented in exampleJsInterop.js
            return jsRuntime.InvokeAsync<string>("localStorageFunction.setItem", key, value);
        }
        public static ValueTask<string> GetItem(IJSRuntime jsRuntime, string key)
        {
            // Implemented in exampleJsInterop.js
            return jsRuntime.InvokeAsync<string>("localStorageFunction.getItem", key);
        }
    }
}
