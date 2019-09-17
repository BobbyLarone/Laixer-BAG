using Dapper;
using LaixerGMLTest.BAG_Objects;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace LaixerGMLTest
{
    internal class DatabaseLoader : ILoader
    {
        public void Load(List<BAGObject> bAGObjects)
        {
            using (var connection = new NpgsqlConnection(""))
            {
                // TOOD: Check to insert which table.

                //var residenceObjects = bAGObjects.Cast<Residence>();

                var residenceObjects = bAGObjects.Cast<PublicSpace>();

                var sql = LoadOPR();

                var orderDetails = connection.Execute(sql, residenceObjects);
            }
        }
        private string LoadWPL()
        {
            return $@"
                    INSERT INTO public.woonplaats(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        woonplaatsnaam,
                        woonplaatsstatus,
                        geom_valid,
                        geovlak)
                    VALUES (
                        @Identification,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @WoonplaatsNaam,
                        @WoonplaatsStatus::woonplaatsstatus,
                        null,
                        ST_Multi(ST_GeomFromGML(@Geovlak, 28992)))";
        }
        private string LoadOPR()
        {
            return @"INSERT INTO public.openbareruimte(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        openbareruimtenaam,
                        openbareruimtestatus,
                        openbareruimtetype,
                        gerelateerdewoonplaats,
                        verkorteopenbareruimtenaam)
                        VALUES
                        (
                        @Identification,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @OpenbareRuimteNaam,
                        @OpenbareruimteStatus::openbareruimtestatus,
                        @OpenbareRuimteType::openbareruimtetype,
                        @GerelateerdeWoonplaats,
                        @VerkorteOpenbareruimteNaam)";
        }

        private void LoadLIG()
        {

        }

        private void LoadNUM()
        {

        }

        private void LoadPND()
        {

        }

        private void LoadSTA()
        {

        }

        private void LoadVBO()
        {

        }
    }
}
