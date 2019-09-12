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

                var residenceObjects = bAGObjects.Cast<Residence>();

                var sql = $@"
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
                        null, -- @BegindatumTijdvakGeldigheid,
                        null, -- @EinddatumTijdvakGeldigheid,
                        @DocumentNummer,
                        null, -- @DocumentDatum,
                        @WoonplaatsNaam,
                        null,
                        null,
                        ST_Multi(ST_GeomFromGML(@Geovlak)))";

                var orderDetails = connection.Execute(sql, residenceObjects);
            }
        }
    }
}
