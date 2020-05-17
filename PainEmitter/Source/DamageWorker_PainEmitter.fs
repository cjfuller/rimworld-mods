namespace PainEmitter

open Verse

type DamageWorker_PainEmitter() =
    inherit DamageWorker()

    override this.Apply(dinfo: DamageInfo, thing: Thing): DamageWorker.DamageResult =
        match thing with
        | :? Pawn as pawn -> this.ApplyToPawn(dinfo, pawn)
        | _ -> base.Apply(dinfo, thing)

    member this.ChooseHitPart(dinfo: DamageInfo, pawn: Pawn) =
        let part =
            pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth)

        if isNull part
        then Log.Warning("ChooseHitPart returning null (any part)")
        part

    member this.GetExactPartFromDamageInfo(dinfo: DamageInfo, pawn: Pawn) =
        let part =
            match dinfo.HitPart with
            | null -> this.ChooseHitPart(dinfo, pawn)
            | _ -> dinfo.HitPart

        if Seq.exists (fun presentPart -> presentPart = part) (pawn.health.hediffSet.GetNotMissingParts())
        then part
        else null

    member this.ApplyToPawn(dinfo: DamageInfo, pawn: Pawn): DamageWorker.DamageResult =
        let damageResult = DamageWorker.DamageResult()

        let exactPart =
            this.GetExactPartFromDamageInfo(dinfo, pawn)

        if (not (isNull exactPart)) then
            do dinfo.SetHitPart(exactPart)
            if (not (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart))) then
                do
                   let hediffDefFromDamage =
                       HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart)

                   let injury =
                       HediffMaker.MakeHediff(hediffDefFromDamage, pawn) :?> Hediff_Injury

                   injury.Part <- dinfo.HitPart
                   injury.source <- dinfo.Weapon
                   injury.sourceHediffDef <- dinfo.WeaponLinkedHediff
                   injury.Severity <- 1.0f
                   pawn.health.AddHediff(injury)
                   damageResult.wounded <- true
                   damageResult.AddPart(pawn, injury.Part)
                   damageResult.AddHediff(injury)

        damageResult
