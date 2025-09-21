1. Import Core repository as submodule in root folder
2. Add in manifest external scopes registrer:
"scopedRegistries":
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.openupm",
        "com.neuecc.unirx",
        "com.cysharp.unitask",
        "com.cysharp.memorypack",
        "com.svermeulen.extenject",
        "com.dbrizov.naughtyattributes",
        "com.system-community.systemruntimecompilerservicesunsafe",
        "com.marijnzwemmer.unity-toolbar-extender"
      ]
    }
3. Install all of them using Package Manager
Attention! MemoryPack should be 1.10.0 only, not abowe!
4. Add define on "UNITASK_DOTWEEN_SUPPORT"
5. Add dependency in manifest "com.tendedtarsier.core": "file:../../core/Core/Assets" 
