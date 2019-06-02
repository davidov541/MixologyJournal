/*
 * Copyright 2016 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package net.davidmcginnis.mixologyjournal.appengine.services.profileinfo;

import com.google.api.server.spi.config.AnnotationBoolean;
import com.google.api.server.spi.config.Api;
import com.google.api.server.spi.config.ApiMethod;
import com.google.api.server.spi.config.ApiNamespace;
import com.google.api.server.spi.config.Named;
import com.google.appengine.api.datastore.*;

/**
 * The Profile Info API which allows users to sync their profiles to the cloud, and then retrieve it later.
 */
@Api(
    name = "profileinfo",
    version = "v1",
    namespace =
    @ApiNamespace(
        ownerDomain = "davidmcginnis.net",
        ownerName = "davidmcginnis.net",
        packagePath = ""
    )
)

public class ProfileInfoService {
    private static String userKindName = "User";

    @ApiMethod(name = "add_user", path = "addUser/{user}", apiKeyRequired = AnnotationBoolean.TRUE)
    public Response addUser(@Named("user") String emailAddress) {
        Entity user = new Entity(userKindName, emailAddress);
        user.setProperty("email", emailAddress);

        DatastoreService datastore = DatastoreServiceFactory.getDatastoreService();
        datastore.put(user);
        return new Response("Success");
    }

    @ApiMethod(name = "user_exists", path = "userExists/{user}", apiKeyRequired = AnnotationBoolean.TRUE)
    public Response userExists(@Named("user") String emailAddress) {
        Entity possibleEntity = new Entity(userKindName, emailAddress);
        DatastoreService datastore = DatastoreServiceFactory.getDatastoreService();
        try {
            datastore.get(possibleEntity.getKey());
        } catch (EntityNotFoundException e) {
            return new Response("Failure");
        }
        return new Response("Success");
    }
}
