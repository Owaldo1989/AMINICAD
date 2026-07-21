ALTER PROCEDURE dbo.SpIngresosControlOperativo
(
    @FechaInicialRegistro DATE = NULL,
    @FechaFinalRegistro DATE = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SET @FechaFinalRegistro = ISNULL(@FechaFinalRegistro, @Hoy);
    SET @FechaInicialRegistro = ISNULL(@FechaInicialRegistro, @FechaFinalRegistro);

    IF @FechaInicialRegistro > @FechaFinalRegistro
        THROW 50001, 'La fecha inicial no puede ser posterior a la fecha final.', 1;

    IF DATEDIFF(DAY, @FechaInicialRegistro, @FechaFinalRegistro) > 30
        THROW 50002, 'El rango máximo permitido es de 31 días.', 1;

    SELECT
        CAST(A.Fecha AS DATE) AS FechaContable,
        CAST(A.Hora AS DATE) AS FechaGrabacion,
        A.Hora AS FechaHoraGrabacion,
        A.NoIngreso,
        NULLIF(LTRIM(RTRIM(CONVERT(NVARCHAR(50), A.NoRecibo))), '') AS NoRecibo,
        A.CodCuenta,
        ISNULL(CC.Cuenta, 'Cuenta no identificada') AS Cuenta,
        ISNULL(A.Total, 0) AS Total,
        DATEDIFF(DAY, CAST(A.Fecha AS DATE), CAST(A.Hora AS DATE)) AS DiasDiferencia
    INTO #IngresosBase
    FROM dbo.tbIngresosMaestros A
    LEFT JOIN dbo.tbCatalogoCuentas CC
        ON A.CodCuenta = CC.CodCuenta
    WHERE A.Hora >= @FechaInicialRegistro
      AND A.Hora < DATEADD(DAY, 1, @FechaFinalRegistro);

    CREATE CLUSTERED INDEX IX_IngresosBase_FechaGrabacion
        ON #IngresosBase(FechaGrabacion, NoIngreso);

    /* RESULTADO 1: resumen acumulado del período. */
    SELECT
        @FechaInicialRegistro AS FechaInicialRegistro,
        @FechaFinalRegistro AS FechaFinalRegistro,
        COUNT(DISTINCT NoIngreso) AS CantidadIngresos,
        COUNT(DISTINCT NoRecibo) AS CantidadRecibosFisicos,
        SUM(Total) AS TotalRegistrado,
        SUM(CASE WHEN CodCuenta = 110001 THEN Total ELSE 0 END) AS TotalCajaGeneral,
        SUM(CASE WHEN CodCuenta <> 110001 OR CodCuenta IS NULL THEN Total ELSE 0 END) AS TotalOtrasCuentas,
        COUNT(DISTINCT CASE WHEN FechaContable = FechaGrabacion THEN NoIngreso END) AS IngresosMismoDia,
        COUNT(DISTINCT CASE WHEN FechaContable < FechaGrabacion THEN NoIngreso END) AS IngresosFechasAnteriores,
        COUNT(DISTINCT CASE WHEN FechaContable > FechaGrabacion THEN NoIngreso END) AS IngresosFechaFutura,
        MIN(FechaContable) AS FechaContableMasAntigua,
        MAX(DiasDiferencia) AS MayorDiferenciaDias,
        AVG(CAST(CASE WHEN DiasDiferencia >= 0 THEN DiasDiferencia END AS DECIMAL(10, 2))) AS PromedioDiasDiferencia,
        COUNT(DISTINCT CodCuenta) AS CantidadCuentasUtilizadas
    FROM #IngresosBase;

    /* RESULTADO 2: actividad agrupada por día de grabación. */
    SELECT
        FechaGrabacion,
        COUNT(DISTINCT NoIngreso) AS CantidadIngresos,
        COUNT(DISTINCT NoRecibo) AS CantidadRecibosFisicos,
        COUNT(DISTINCT FechaContable) AS CantidadFechasContables,
        SUM(Total) AS TotalRegistrado,
        SUM(CASE WHEN CodCuenta = 110001 THEN Total ELSE 0 END) AS TotalCajaGeneral,
        SUM(CASE WHEN CodCuenta <> 110001 OR CodCuenta IS NULL THEN Total ELSE 0 END) AS TotalOtrasCuentas,
        COUNT(DISTINCT CASE WHEN FechaContable < FechaGrabacion THEN NoIngreso END) AS IngresosFechasAnteriores,
        COUNT(
            DISTINCT CASE
                WHEN FechaContable > FechaGrabacion
                  OR NoRecibo IS NULL
                  OR CodCuenta IS NULL
                  OR Cuenta = 'Cuenta no identificada'
                  OR (CodCuenta = 110001 AND DiasDiferencia > 0)
                  OR DiasDiferencia > 3
                    THEN NoIngreso
            END
        ) AS CantidadAlertas
    FROM #IngresosBase
    GROUP BY FechaGrabacion
    ORDER BY FechaGrabacion DESC;

    /* RESULTADO 3: fechas contables procesadas dentro del período. */
    SELECT
        FechaContable,
        MAX(DiasDiferencia) AS DiasDiferencia,
        COUNT(DISTINCT NoIngreso) AS CantidadIngresos,
        COUNT(DISTINCT NoRecibo) AS CantidadRecibosFisicos,
        SUM(Total) AS TotalRegistrado,
        SUM(CASE WHEN CodCuenta = 110001 THEN Total ELSE 0 END) AS TotalCajaGeneral,
        SUM(CASE WHEN CodCuenta <> 110001 OR CodCuenta IS NULL THEN Total ELSE 0 END) AS TotalOtrasCuentas,
        MIN(FechaHoraGrabacion) AS PrimeraGrabacion,
        MAX(FechaHoraGrabacion) AS UltimaGrabacion
    FROM #IngresosBase
    GROUP BY FechaContable
    ORDER BY FechaContable DESC;

    /* RESULTADO 4: distribución por cuenta de todo el período. */
    ;WITH Cuentas AS
    (
        SELECT
            CodCuenta,
            Cuenta,
            COUNT(DISTINCT NoIngreso) AS CantidadIngresos,
            COUNT(DISTINCT NoRecibo) AS CantidadRecibosFisicos,
            SUM(Total) AS TotalRegistrado,
            MIN(FechaContable) AS FechaContableMasAntigua,
            MAX(DiasDiferencia) AS MayorDiferenciaDias
        FROM #IngresosBase
        GROUP BY CodCuenta, Cuenta
    ),
    TotalGeneral AS
    (
        SELECT SUM(TotalRegistrado) AS Total
        FROM Cuentas
    )
    SELECT
        C.CodCuenta,
        C.Cuenta,
        C.CantidadIngresos,
        C.CantidadRecibosFisicos,
        C.TotalRegistrado,
        CAST(
            CASE
                WHEN ISNULL(TG.Total, 0) = 0 THEN 0
                ELSE C.TotalRegistrado * 100.0 / TG.Total
            END
            AS DECIMAL(10, 2)
        ) AS PorcentajeParticipacion,
        C.FechaContableMasAntigua,
        C.MayorDiferenciaDias,
        CASE WHEN C.CodCuenta = 110001 THEN 1 ELSE 0 END AS EsCajaGeneral
    FROM Cuentas C
    CROSS JOIN TotalGeneral TG
    ORDER BY C.TotalRegistrado DESC;

    /* RESULTADO 5: alertas acumuladas del período. */
    SELECT
        COUNT(DISTINCT CASE WHEN NoRecibo IS NULL THEN NoIngreso END) AS IngresosSinReciboFisico,
        COUNT(DISTINCT CASE WHEN CodCuenta IS NULL THEN NoIngreso END) AS IngresosSinCuenta,
        COUNT(DISTINCT CASE WHEN Cuenta = 'Cuenta no identificada' THEN NoIngreso END) AS IngresosCuentaNoIdentificada,
        COUNT(DISTINCT CASE WHEN FechaContable > FechaGrabacion THEN NoIngreso END) AS IngresosFechaPosterior,
        COUNT(DISTINCT CASE WHEN DiasDiferencia > 3 THEN NoIngreso END) AS IngresosConMasTresDias,
        COUNT(DISTINCT CASE WHEN CodCuenta = 110001 AND DiasDiferencia > 0 THEN NoIngreso END) AS CajaGeneralConFechaAnterior
    FROM #IngresosBase;

    /* RESULTADO 6: registros que requieren revisión. */
    SELECT
        NoIngreso,
        NoRecibo,
        FechaContable,
        FechaHoraGrabacion,
        DiasDiferencia,
        CodCuenta,
        Cuenta,
        Total,
        CASE
            WHEN FechaContable > FechaGrabacion THEN 'Fecha contable posterior al registro'
            WHEN NoRecibo IS NULL THEN 'Sin número de recibo físico'
            WHEN CodCuenta IS NULL THEN 'Sin cuenta contable'
            WHEN Cuenta = 'Cuenta no identificada' THEN 'Cuenta no encontrada en catálogo'
            WHEN CodCuenta = 110001 AND DiasDiferencia > 0 THEN 'Caja General con fecha anterior'
            WHEN DiasDiferencia > 3 THEN 'Diferencia mayor de tres días'
            ELSE 'Revisar'
        END AS MotivoRevision
    FROM #IngresosBase
    WHERE FechaContable > FechaGrabacion
       OR NoRecibo IS NULL
       OR CodCuenta IS NULL
       OR Cuenta = 'Cuenta no identificada'
       OR (CodCuenta = 110001 AND DiasDiferencia > 0)
       OR DiasDiferencia > 3
    ORDER BY FechaHoraGrabacion DESC, DiasDiferencia DESC;

    DROP TABLE #IngresosBase;
END;
