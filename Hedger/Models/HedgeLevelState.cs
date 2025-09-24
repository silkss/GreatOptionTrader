namespace Hedger.Models;

public enum HedgeLevelState {
    /// <summary>
    /// Ждем когда сработает ActivatePrice для покупки / продажи.
    /// </summary>
    Observe,

    /// <summary>
    /// Купили/продали и ждем дальше. Либо закрываем по AntiStopPrice или переходим в Lossless при достижении LossLess уровня.
    /// </summary>
    InPosition, 

    /// <summary>
    /// Добрались до Lossless уровня. теперь закрывемся если достигнем цены открытия.
    /// </summary>
    InLossLess,

    /// <summary>
    /// Закрыли позу или по лосслесу, или по антистоп уровню. и ждем достижения нужного уровня чтобы опять начать все сначала.
    /// </summary>
    InRestart, 
}
