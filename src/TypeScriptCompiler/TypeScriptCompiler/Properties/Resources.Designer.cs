﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TypeScript.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TypeScript.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to var libfile = &apos;/// &lt;reference no-default-lib=&quot;true&quot;/&gt;\n&apos; +
        ///                 function (p, a, c, k, e, r) { e = function (c) { return (c &lt; a ? &apos;&apos; : e(parseInt(c / a))) + ((c = c % a) &gt; 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!&apos;&apos;.replace(/^/, String)) { while (c--) r[e(c)] = k[c] || e(c); k = [function (e) { return r[e] }]; e = function () { return &apos;\\w+&apos; }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp(&apos;\\b&apos; + e(c) + &apos;\\b&apos;, &apos;g&apos;), k[c]); return p }(&apos;4 5 dW:0;\n4 5 qt:0;\n4 B qu(x:1 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string compiler_js {
            get {
                return ResourceManager.GetString("compiler_js", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /* *****************************************************************************
        ///Copyright (c) Microsoft Corporation. All rights reserved. 
        ///Licensed under the Apache License, Version 2.0 (the &quot;License&quot;); you may not use
        ///this file except in compliance with the License. You may obtain a copy of the
        ///License at http://www.apache.org/licenses/LICENSE-2.0  
        /// 
        ///THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
        ///KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIE [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string typescript_js {
            get {
                return ResourceManager.GetString("typescript_js", resourceCulture);
            }
        }
    }
}