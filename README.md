# Apigee Explorer API V2

Esta es la versión mejorada del backend de Apigee Explorer con una arquitectura más organizada y modular.

## 🏗️ Estructura del Proyecto

```
ApigeeExplorer.ApiV2/
├── Controllers/                 # Controladores separados por responsabilidad
│   ├── SearchController.cs     # Operaciones de búsqueda
│   └── HealthController.cs     # Health checks
├── Models/                     # Modelos organizados por categoría
│   ├── Core/                   # Modelos principales
│   │   ├── SearchResult.cs
│   │   └── SearchResultDetails.cs
│   ├── Entities/               # Entidades de Apigee
│   │   ├── Developer.cs
│   │   ├── App.cs
│   │   ├── Product.cs
│   │   ├── ApiProxy.cs
│   │   ├── TargetServer.cs
│   │   ├── Keystore.cs
│   │   └── Reference.cs
│   └── Enriched/               # Modelos enriquecidos
│       ├── EnrichedApp.cs
│       ├── EnrichedProductApp.cs
│       ├── EnrichedProxy.cs
│       ├── EnrichedEnvironment.cs
│       └── EnrichedFlow.cs
├── Services/                   # Servicios organizados por funcionalidad
│   ├── Interfaces/             # Interfaces de servicios
│   │   ├── IDataLoaderService.cs
│   │   └── ISearchService.cs
│   ├── SearchServices/         # Servicios de búsqueda específicos
│   │   ├── DeveloperSearchService.cs
│   │   ├── AppSearchService.cs
│   │   └── ProductSearchService.cs
│   ├── DataLoaderService.cs    # Carga de datos
│   └── SearchService.cs        # Coordinador de búsquedas
├── Configuration/              # Configuración de la aplicación
│   └── ServiceCollectionExtensions.cs
├── data/                       # Datos JSON (copiados del original)
│   ├── AWS/
│   └── ONP/
├── Properties/
│   └── launchSettings.json
├── Program.cs                  # Punto de entrada
├── appsettings.json
└── appsettings.Development.json
```

## 🚀 Principales Mejoras

### 1. **Separación de Responsabilidades**
- **Controladores**: Cada controlador tiene una responsabilidad específica
- **Modelos**: Organizados por categoría (Core, Entities, Enriched)
- **Servicios**: Separados por funcionalidad y tipo de componente

### 2. **Servicios Modulares**
- **DataLoaderService**: Responsable solo de cargar datos
- **SearchServices**: Servicios específicos para cada tipo de componente
- **SearchService**: Coordinador principal que orquesta las búsquedas

### 3. **Interfaces Bien Definidas**
- Contratos claros entre servicios
- Facilita testing y mocking
- Mejor inversión de dependencias

### 4. **Configuración Organizada**
- Extensions methods para configuración
- Separación de concerns en Program.cs
- Configuración centralizada

### 5. **Mejor Performance**
- Búsquedas paralelas
- Carga asíncrona de datos
- Mejor manejo de memoria

### 6. **Logging Estructurado**
- Logs específicos por servicio
- Diferentes niveles de logging
- Mejor trazabilidad

## 🔧 Configuración y Uso

### Prerrequisitos
- .NET 9.0 SDK
- Los archivos de datos del proyecto original

### Instalación

1. **Copiar datos**:
   ```bash
   # Copiar la carpeta data del proyecto original
   cp -r ../ApigeeExplorer.Api/data ./data
   ```

2. **Restaurar paquetes**:
   ```bash
   dotnet restore
   ```

3. **Ejecutar**:
   ```bash
   dotnet run
   ```

### Endpoints

- **Health Check**: `GET /api/health`
- **Environments**: `GET /api/search/environments`
- **Search**: `GET /api/search/{environment}/{searchTerm}`

### Configuración de Puerto

La aplicación corre en el puerto **5001** (diferente del original 5000) para evitar conflictos.

## 🧪 Testing

```bash
# Ejecutar tests unitarios
dotnet test

# Health check
curl http://localhost:5001/api/health

# Búsqueda de ejemplo
curl http://localhost:5001/api/search/AWS/test
```

## 📊 Comparación con la Versión Original

| Aspecto | Original | V2 |
|---------|----------|-----|
| **Controladores** | 1 archivo grande | Separados por responsabilidad |
| **Modelos** | 2 archivos mezclados | Organizados por categoría |
| **Servicios** | 1 servicio monolítico | Servicios modulares |
| **Configuración** | Todo en Program.cs | Extensions organizadas |
| **Performance** | Búsquedas secuenciales | Búsquedas paralelas |
| **Testing** | Difícil de testear | Fácil mocking y testing |
| **Mantenimiento** | Código acoplado | Bajo acoplamiento |

## 🔄 Migración desde V1

1. **Datos**: Copiar carpeta `data/` completa
2. **Puerto**: Cambiar frontend para usar puerto 5001
3. **Funcionalidad**: Mantiene 100% compatibilidad con la API original

## 🛠️ Desarrollo

### Agregar Nuevo Tipo de Componente

1. **Crear entidad** en `Models/Entities/`
2. **Crear servicio de búsqueda** en `Services/SearchServices/`
3. **Registrar servicio** en `ServiceCollectionExtensions.cs`
4. **Agregar al SearchService** principal

### Estructura de Commits

- `feat:` Nueva funcionalidad
- `fix:` Corrección de bugs
- `refactor:` Refactoring de código
- `docs:` Documentación
- `test:` Tests

## 📈 Próximas Mejoras

- [ ] Cache distribuido (Redis)
- [ ] Paginación de resultados
- [ ] Filtros avanzados
- [ ] Métricas y monitoring
- [ ] Tests unitarios completos
- [ ] Docker containerization
- [ ] CI/CD pipeline

## 🤝 Contribución

1. Fork del proyecto
2. Crear feature branch
3. Commit cambios
4. Push al branch
5. Crear Pull Request

---

**Versión**: 2.0.0  
**Puerto**: 5001  
**Compatibilidad**: 100% con API V1