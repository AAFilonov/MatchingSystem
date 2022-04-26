using System.Linq.Expressions;
using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Algorithms.impl;

/*
// Изначально все мужчины не женаты и все женщины незамужние.
while существует свободный мужчина
M = некоторый свободный мужчина
w = первая женщина из текущего списка M
if w свободна
 помечаем M и w помолвленными
else if w предпочитает M своему текущему жениху M'
 помечаем M и w помолвленными
 вычёркиваем w из списка предпочтений M'
 помечаем M' свободным
else
 вычёркиваем w из списка предпочтений M
 */

//Классический Алгоритм Гейла Шепли
public class DAAAlgorithm<T, U> : IOneToOneAllocationAlgorithm<T, U>
    where T : class, ISolitaryAllocated
    where U : class, ISolitaryAllocated
{
    public void computeStep(OneToOneAllocation<T, U> allocation)
    {
        var freeMen = allocation.active.Where(allocated => HaveReacheblePair(allocated,allocation));
        foreach (var freeMan in freeMen)
        {
            findPair(allocation, freeMan);
        }
    }

    private static void findPair(OneToOneAllocation<T, U> allocation, T freeMan)
    {
        var DesiredWomen = allocation.getPreferencesActive(freeMan);
        if (!DesiredWomen.Any())
            return; //список предпочтений пуст
        var DesiredWoman = DesiredWomen.First();

        var womanPref = allocation.getPreferencesPassive(DesiredWoman);
        var womanPair = allocation.getPairT(DesiredWoman);
        if (womanPair == null)
        {
            allocation.setPair(DesiredWoman, freeMan);
        }
        else if (womanPref.IndexOf(freeMan) > -1 &&
                 womanPref.IndexOf(freeMan) < womanPref.IndexOf(womanPair)) //новый мужчина лучше предыдущего
        {
            allocation.setPair(DesiredWoman, freeMan);
            allocation.deleteFromPref(womanPair, DesiredWoman);
        }
        else
        {
            allocation.deleteFromPref(freeMan, DesiredWoman);
        }
    }


    public bool isFinal(OneToOneAllocation<T, U> allocation)
    {
        var freeMen = allocation.active.Where(allocated => HaveReacheblePair(allocated,allocation));
        return !freeMen.Any();
    }

    private bool HaveReacheblePair(T allocated, OneToOneAllocation<T, U> allocation)
    {
        bool havePair = allocation.pairs.ContainsValue(allocated);
        bool canFindPair = allocation.getPreferencesActive(allocated).Any();
        return !havePair && canFindPair;
    }
}