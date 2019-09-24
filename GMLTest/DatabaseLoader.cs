using Dapper;
using LaixerGMLTest.BAG_Objects;
using LaixerGMLTest.Object_Relations;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaixerGMLTest
{
    internal class DatabaseLoader : ILoader
    {
        /// <summary>
        /// Load the BAGObjects Async
        /// </summary>
        /// <param name="bAGObjects">The BAGobjects to load</param>
        /// <returns></returns>
        public async Task LoadAsync(List<BAGObject> bAGObjects)
        {
            // Add connection details here
            using (var connection = new NpgsqlConnection(""))
            {
                // if the object count is 0
                if (bAGObjects.Count == 0)
                {
                    // something went wrong, soo throw an exception
                    throw new System.Exception("XYZ");
                }

                // First check what type the first object is.
                var firstItem = bAGObjects[0];

                // FUTURE: We can improve this in C# 8.0

                if (firstItem is Residence)
                {
                    // load parameters for "Woonplaats"
                    var loadableObjects = LoadWPL(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is PublicSpace)
                {
                    // load parameters for "Openbare ruimte"
                    var loadableObjects = LoadOPR(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is Berth)
                {
                    // load parameters for "Ligplaats"
                    var loadableObjects = LoadLIG(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is NumberIndication)
                {
                    // load parameters for "Nummer indicatie"
                    var loadableObjects = LoadNUM(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is Premises)
                {
                    // load parameters for "Pand"
                    var loadableObjects = LoadPND(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is Location)
                {
                    // load parameters for "Standplaats"
                    var loadableObjects = LoadSTA(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is Accommodation)
                {
                    // load parameters for "Verblijfs object"
                    var loadableObjects = LoadVBO(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
                else if (firstItem is MunicipalityResidenceRelation)
                {
                    // load parameters for "Gemeente-Woonplaats relatie"
                    var loadableObjects = LoadGWR(bAGObjects, out string sql);
                    await connection.ExecuteAsync(sql, loadableObjects);
                }
            }
        }

        /// <summary>
        /// Loads the Residence (Woonplaats) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<Residence> LoadWPL(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = $@"
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
                        @Identificatie,
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

            return bAGObjects.Cast<Residence>();
        }
        /// <summary>
        /// Loads the Public space (Openbare ruimte) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<PublicSpace> LoadOPR(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = @"
                    INSERT INTO public.openbareruimte(
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
                        @Identificatie,
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
            return bAGObjects.Cast<PublicSpace>();
        }

        /// <summary>
        /// Loads the Berth (Ligplaats) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<Berth> LoadLIG(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = @"
                    INSERT INTO public.ligplaats(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        hoofdadres,
                        ligplaatsstatus,
                        geom_valid,
                        geovlak)
                    VALUES
                        (
                        @Identificatie,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @Hoofdadres,
                        @Ligplaatsstatus::ligplaatsstatus,
                        null,
                        ST_GeomFromGML(@Geovlak, 28992))";


            return bAGObjects.Cast<Berth>();
        }
        /// <summary>
        /// Loads the Number Indication (Nummer Indicatie) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<NumberIndication> LoadNUM(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = @"
                    INSERT INTO public.nummeraanduiding(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        huisnummer,
                        huisletter,
                        huisnummertoevoeging,
                        postcode,
                        nummeraanduidingstatus,
                        typeadresseerbaarobject,
                        gerelateerdeopenbareruimte,
                        gerelateerdewoonplaats)
                        VALUES
                        (
                        @Identificatie,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @Huisnummer::numeric,
                        @Huisletter,
                        @Huisnummertoevoeging,
                        @Postcode,
                        @Nummeraanduidingstatus::nummeraanduidingstatus,
                        @Typeadresseerbaarobject::typeadresseerbaarobject,
                        @Gerelateerdeopenbareruimte,
                        @Gerelateerdewoonplaats)";
            return bAGObjects.Cast<NumberIndication>();
        }

        /// <summary>
        /// Loads the Premises (Panden) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<Premises> LoadPND(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = @"
                    INSERT INTO public.pand(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        pandstatus,
                        bouwjaar,
                        geom_valid,
                        geovlak)
                        VALUES
                        (
                        @Identificatie,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        NULLIF(@EinddatumTijdvakGeldigheid::timestamptz,'0001-01-01 00:00:00+00'::timestamptz),
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @Pandstatus::pandstatus,
                        @Bouwjaar::numeric,
                        null,
                        ST_GeomFromGML(@Geovlak, 28992))";
            return bAGObjects.Cast<Premises>();
        }

        /// <summary>
        /// Loads the Location (standplaats) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<Location> LoadSTA(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = @"
                    INSERT INTO public.standplaats(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        hoofdadres,
                        standplaatsstatus,
                        geom_valid,
                        geovlak)
                    VALUES
                        (
                        @Identificatie,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @Hoofdadres,
                        @Standplaatsstatus::standplaatsstatus,
                        null,
                        ST_GeomFromGML(@Geovlak, 28992))";

            return bAGObjects.Cast<Location>();
        }
        /// <summary>
        /// Loads the Accomodation(Verblijfs objecten) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<Accommodation> LoadVBO(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = $@"
                    INSERT INTO public.verblijfsobject(
                        identificatie,
                        aanduidingrecordinactief,
                        aanduidingrecordcorrectie,
                        officieel,
                        inonderzoek,
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        documentnummer,
                        documentdatum,
                        hoofdadres,
                        verblijfsobjectstatus,
                        oppervlakteverblijfsobject,
                        geom_valid,
                        geopunt,
                        geovlak)
                    VALUES (
                        @Identificatie,
                        @AanduidingRecordInactief::boolean,
                        @AanduidingRecordCorrectie::int,
                        @Officieel::boolean,
                        @InOnderzoek::boolean,
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @DocumentNummer,
                        @DocumentDatum::date,
                        @Hoofdadres,
                        @Verblijfsobjectstatus::verblijfsobjectstatus,
                        @Oppervlakteverblijfsobject::numeric,
                        null,
                        CASE 
                            WHEN (@Geopunt IS NULL) THEN NULL
                            ELSE ST_GeomFromGML(@Geopunt, 28992)
                        END,
                        CASE WHEN (@Geovlak IS NULL) THEN NULL 
                            ELSE ST_GeomFromGML(@Geovlak, 28992)
                        END)";

            return bAGObjects.Cast<Accommodation>();
        }

        /// <summary>
        /// Loads the Accomodation(Verblijfs objecten) objects and also provides a sql query
        /// </summary>
        /// <param name="bAGObjects">The BAG objects to transform</param>
        /// <param name="sqlstring">The SQL paramater that is created for this</param>
        /// <returns>The converted BAG objects</returns>
        private IEnumerable<MunicipalityResidenceRelation> LoadGWR(List<BAGObject> bAGObjects, out string sqlstring)
        {
            sqlstring = $@"
                    INSERT INTO public.gemeente_woonplaats(
                        begindatumtijdvakgeldigheid,
                        einddatumtijdvakgeldigheid,
                        woonplaatscode,
                        gemeentecode,
                        status)
                    VALUES (
                        @BegindatumTijdvakGeldigheid::timestamptz,
                        @EinddatumTijdvakGeldigheid::timestamptz,
                        @Woonplaatscode,
                        @Gemeentecode,
                        @Status::gemeentewoonplaatsstatus)";

            return bAGObjects.Cast<MunicipalityResidenceRelation>();
        }
    }
}
